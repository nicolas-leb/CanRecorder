using CanRecorder.Models;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using vxlapi_NET;

namespace CanRecorder.Services
{
    public class CanVectorService
    {
        public event EventHandler<Frame> FrameReceived;

        private bool _isStarted = false;
        private readonly XLDriver _driver;
        private int _portHandle = XLDefine.XL_INVALID_PORTHANDLE;
        private Thread? _rxThread;
        private EventWaitHandle _waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, null);
        
        public CanVectorService()
        {
            _driver = new XLDriver();
        }
        public IEnumerable<string> GetChannelsName()
        {
            var status = _driver.XL_OpenDriver();
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return Enumerable.Empty<string>();
            }

            XLClass.xl_driver_config driverConfig = new();
            status = _driver.XL_GetDriverConfig(ref driverConfig);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return Enumerable.Empty<string>();
            }

            List<string> channelsName = new();
            for (int i = 0; i < driverConfig.channelCount; i++)
            {
                if ((driverConfig.channel[i].channelBusCapabilities & XLDefine.XL_BusCapabilities.XL_BUS_ACTIVE_CAP_CAN) != 0)
                {
                    channelsName.Add(driverConfig.channel[i].name);
                }
            }
            return channelsName;
        }

        public bool Start(string channelName)
        {
            if (_isStarted)
            {
                return false;
            }

            XLClass.xl_driver_config driverConfig = new();
            var status = _driver.XL_GetDriverConfig(ref driverConfig);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            XLClass.xl_channel_config? channelConfig = null;
            for (int i = 0; i < driverConfig.channelCount; i++)
            {
                if (driverConfig.channel[i].name.Replace("\0", "") == channelName)
                {
                    channelConfig = driverConfig.channel[i];
                    break;
                }
            }
            if (channelConfig == null)
            {
                return false;
            }

            status = _driver.XL_SetApplConfig("CanRecorder", 0, channelConfig.hwType, channelConfig.hwIndex, channelConfig.hwChannel, XLDefine.XL_BusTypes.XL_BUS_TYPE_CAN);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            var channelMask = _driver.XL_GetChannelMask(channelConfig.hwType, channelConfig.hwIndex, channelConfig.hwChannel);

            ulong permissionMask = channelMask;
            status = _driver.XL_OpenPort(ref _portHandle, "CanRecorder", channelMask, ref permissionMask, 256, XLDefine.XL_InterfaceVersion.XL_INTERFACE_VERSION, XLDefine.XL_BusTypes.XL_BUS_TYPE_CAN);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            XLClass.xl_chip_params chipParams = new()
            {
                bitrate = 500000
            };
            status = _driver.XL_CanSetChannelParams(_portHandle, channelMask, chipParams);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            status = _driver.XL_ActivateChannel(_portHandle, channelMask, XLDefine.XL_BusTypes.XL_BUS_TYPE_CAN, XLDefine.XL_AC_Flags.XL_ACTIVATE_NONE);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            int tempInt = -1;
            status = _driver.XL_SetNotification(_portHandle, ref tempInt, 1);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }
            _waitHandle.SafeWaitHandle = new SafeWaitHandle(new IntPtr(tempInt), true);

            status = _driver.XL_ResetClock(_portHandle);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            _rxThread = new Thread(new ThreadStart(RXThread));
            _rxThread.Start();

            _isStarted = true;

            return true;
        }

        public bool Stop()
        {
            if (!_isStarted)
            {
                return false;
            }

            var status = _driver.XL_ClosePort(_portHandle);
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            status = _driver.XL_CloseDriver();
            if (status != XLDefine.XL_Status.XL_SUCCESS)
            {
                return false;
            }

            _isStarted = false;
            return true;
        }

        public void RXThread()
        {
            // Create new object containing received data 
            XLClass.xl_event receivedEvent = new XLClass.xl_event();

            // Result of XL Driver function calls
            XLDefine.XL_Status xlStatus = XLDefine.XL_Status.XL_SUCCESS;


            // Note: this thread will be destroyed by MAIN
            while (_isStarted)
            {
                // Wait for hardware events
                if (_waitHandle.WaitOne(1000))
                {
                    // ...init xlStatus first
                    xlStatus = XLDefine.XL_Status.XL_SUCCESS;

                    // afterwards: while hw queue is not empty...
                    while (xlStatus != XLDefine.XL_Status.XL_ERR_QUEUE_IS_EMPTY)
                    {
                        // ...receive data from hardware.
                        xlStatus = _driver.XL_Receive(_portHandle, ref receivedEvent);

                        //  If receiving succeed....
                        if (xlStatus == XLDefine.XL_Status.XL_SUCCESS)
                        {
                            if ((receivedEvent.flags & XLDefine.XL_MessageFlags.XL_EVENT_FLAG_OVERRUN) != 0)
                            {
                                Console.WriteLine("-- XL_EVENT_FLAG_OVERRUN --");
                            }

                            // ...and data is a Rx msg...
                            if (receivedEvent.tag == XLDefine.XL_EventTags.XL_RECEIVE_MSG)
                            {
                                if ((receivedEvent.tagData.can_Msg.flags & XLDefine.XL_MessageFlags.XL_CAN_MSG_FLAG_OVERRUN) != 0)
                                {
                                    Console.WriteLine("-- XL_CAN_MSG_FLAG_OVERRUN --");
                                }

                                // ...check various flags
                                if ((receivedEvent.tagData.can_Msg.flags & XLDefine.XL_MessageFlags.XL_CAN_MSG_FLAG_ERROR_FRAME)
                                    == XLDefine.XL_MessageFlags.XL_CAN_MSG_FLAG_ERROR_FRAME)
                                {
                                    Console.WriteLine("ERROR FRAME");
                                }

                                else if ((receivedEvent.tagData.can_Msg.flags & XLDefine.XL_MessageFlags.XL_CAN_MSG_FLAG_REMOTE_FRAME)
                                    == XLDefine.XL_MessageFlags.XL_CAN_MSG_FLAG_REMOTE_FRAME)
                                {
                                    Console.WriteLine("REMOTE FRAME");
                                }

                                else
                                {
                                    Frame frame = new Frame(receivedEvent.timeStamp, receivedEvent.tagData.can_Msg.id, receivedEvent.tagData.can_Msg.data.ToList());
                                    EventHandler<Frame> handler = FrameReceived;
                                    if (handler != null)
                                    {
                                        handler(this, frame);
                                    }
                                }
                            }
                        }
                    }
                }
                // No event occurred
            }
        }
    }
}
