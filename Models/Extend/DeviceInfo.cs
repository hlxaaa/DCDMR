using Common.Helper;
using HHTDCDMR.Models.Extend.Req;
using System;

namespace DbOpertion.Models
{
    public partial class DeviceInfo
    {
        public DeviceInfo(EstablishReq req, int userId)
        {
            meterTypeNo = req.meterTypeNo;
            switch (meterTypeNo)
            {
                //一体机计量，必然用ic卡，计量
                case "11":
                    IsIC = 1;
                    moneyOrVolume = 1;
                    break;
                //一体机计费，必然用ic卡，计费
                case "13":
                    IsIC = 1;
                    moneyOrVolume = 0;
                    break;
                //鸿鹄RTU，信东RTU1，信东RTU2，带不带ic都行，带了ic，计量计费都行
                default:
                    moneyOrVolume = req.moneyOrVolume;
                    IsIC = req.IsIC;
                    break;
            }


            LinkMode = req.LinkMode;
            CommMode = req.CommMode;
            if (userId == 1)
                isEncrypt = req.isEncrypt;

            if (req.deviceNo != null)
            {
                deviceNo = StringHelper.Instance.GetLastStr(req.deviceNo, 15);
            }
            if (req.meterNo != null)
                meterNo = Convert.ToInt32(req.meterNo);
            if (req.communicateNo != null)
                communicateNo = req.communicateNo;
            //if (req.barCode != null)
            //    barCode = req.barCode;

            if (req.factoryNo != null)
                factoryNo = req.factoryNo;
            //if (req.openState != null)
            //    openState = Convert.ToInt32(req.openState);
            if (req.caliber != null)
                caliber = req.caliber;
            if (req.baseVolume != null)
                baseVolume = Convert.ToDecimal(req.baseVolume);
            if (req.fluidNo != null)
                fluidNo = req.fluidNo;
            if (req.remark != null)
                remark = req.remark;
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            if (req.collectorNo != null)
                collectorNo = req.collectorNo;
            if (req.AlarmTimes != null)
                AlarmTimes = Convert.ToInt32(req.AlarmTimes);
            if (req.AlarmInvTime != null)
                AlarmInvTime = Convert.ToInt32(req.AlarmInvTime);
            if (req.TempUpper != null)
                TempUpper = Convert.ToDecimal(req.TempUpper);
            if (req.TempLow != null)
                TempLow = Convert.ToDecimal(req.TempLow);
            if (req.PressUpper != null)
                PressUpper = Convert.ToDecimal(req.PressUpper);
            if (req.PressLow != null)
                PressLow = Convert.ToDecimal(req.PressLow);
            if (req.StdFlowUpper != null)
                StdFlowUpper = Convert.ToDecimal(req.StdFlowUpper);
            if (req.StdFlowLow != null)
                StdFlowLow = Convert.ToDecimal(req.StdFlowLow);
            if (req.WorkFlowUpper != null)
                WorkFlowUpper = Convert.ToDecimal(req.WorkFlowUpper);
            if (req.WorkFlowLow != null)
                WorkFlowLow = Convert.ToDecimal(req.WorkFlowLow);
            if (req.RemainMoneyLow != null)
                RemainMoneyLow = Convert.ToDecimal(req.RemainMoneyLow);
            if (req.RemainVolumLow != null)
                RemainVolumLow = Convert.ToDecimal(req.RemainVolumLow);
            if (req.OverMoneyUpper != null)
                OverMoneyUpper = Convert.ToDecimal(req.OverMoneyUpper);
            if (req.OverVolumeUpper != null)
                OverVolumeUpper = Convert.ToDecimal(req.OverVolumeUpper);
            if (req.DoorAlarm != null)
                DoorAlarm = Convert.ToInt32(req.DoorAlarm);
            if (req.PowerUpper != null)
                PowerUpper = Convert.ToDecimal(req.PowerUpper);
            if (req.PowerLow != null)
                PowerLow = Convert.ToDecimal(req.PowerLow);
            if (req.BatteryLow != null)
                BatteryLow = Convert.ToDecimal(req.BatteryLow);

        }

        public DeviceInfo(DeviceInfoReq req, int userId)
        {
            if (userId == 1)
                isEncrypt = req.isEncrypt;
            moneyOrVolume = req.moneyOrVolume;
            IsIC = req.IsIC;
            if (req.lat != null)
                lat = req.lat;
            if (req.lng != null)
                lng = req.lng;

            //if (req.meterNo != null)
            //    meterNo = Convert.ToInt32(req.meterNo);
            if (req.deviceNo != null)
                deviceNo = StringHelper.Instance.GetLastStr(req.deviceNo, 15);
            if (req.communicateNo != null)
                communicateNo = req.communicateNo;
            //if (req.barCode != null)
            //    barCode = req.barCode;
            if (req.meterTypeNo != null)
                meterTypeNo = req.meterTypeNo;

            if (req.factoryNo != null)
                factoryNo = req.factoryNo;
            if (req.openState != null)
                openState = Convert.ToInt32(req.openState);
            if (req.caliber != null)
                caliber = req.caliber;
            //if (req.baseVolume != null)
            //    baseVolume = Convert.ToDecimal(req.baseVolume);
            if (req.fluidNo != null)
                fluidNo = req.fluidNo;
            if (req.remark != null)
                remark = req.remark;
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            //if (req.collectorNo != null)
            //    collectorNo = req.collectorNo;
            if (req.AlarmTimes != null)
                AlarmTimes = Convert.ToInt32(req.AlarmTimes);
            if (req.AlarmInvTime != null)
                AlarmInvTime = Convert.ToInt32(req.AlarmInvTime);
            if (req.TempUpper != null)
                TempUpper = Convert.ToDecimal(req.TempUpper);
            if (req.TempLow != null)
                TempLow = Convert.ToDecimal(req.TempLow);
            if (req.PressUpper != null)
                PressUpper = Convert.ToDecimal(req.PressUpper);
            if (req.PressLow != null)
                PressLow = Convert.ToDecimal(req.PressLow);
            if (req.StdFlowUpper != null)
                StdFlowUpper = Convert.ToDecimal(req.StdFlowUpper);
            if (req.StdFlowLow != null)
                StdFlowLow = Convert.ToDecimal(req.StdFlowLow);
            if (req.WorkFlowUpper != null)
                WorkFlowUpper = Convert.ToDecimal(req.WorkFlowUpper);
            if (req.WorkFlowLow != null)
                WorkFlowLow = Convert.ToDecimal(req.WorkFlowLow);
            if (req.RemainMoneyLow != null)
                RemainMoneyLow = Convert.ToDecimal(req.RemainMoneyLow);
            if (req.RemainVolumLow != null)
                RemainVolumLow = Convert.ToDecimal(req.RemainVolumLow);
            if (req.OverMoneyUpper != null)
                OverMoneyUpper = Convert.ToDecimal(req.OverMoneyUpper);
            if (req.OverVolumeUpper != null)
                OverVolumeUpper = Convert.ToDecimal(req.OverVolumeUpper);
            if (req.DoorAlarm != null)
                DoorAlarm = Convert.ToInt32(req.DoorAlarm);
            if (req.PowerUpper != null)
                PowerUpper = Convert.ToDecimal(req.PowerUpper);
            if (req.PowerLow != null)
                PowerLow = Convert.ToDecimal(req.PowerLow);
            if (req.BatteryLow != null)
                BatteryLow = Convert.ToDecimal(req.BatteryLow);
            if (req.ProtocolNo != null)
                ProtocolNo = req.ProtocolNo;
            if (req.lat != null)
                lat = req.lat;
            if (req.lng != null)
                lng = req.lng;
            if (req.CommMode != null)
                CommMode = Convert.ToInt32(req.CommMode);
            if (req.LinkMode != null)
                LinkMode = Convert.ToInt32(req.LinkMode);
            if (req.IsIC != null)
                IsIC = req.IsIC;
        }

        public DeviceInfo(DeviceUpdateReq req, int userId)
        {
            if (req.ScadaInvTime != null)
                ScadaInvTime = req.ScadaInvTime;
            if (userId == 1)
                isEncrypt = req.isEncrypt;
            //isEncrypt = req.isEncrypt;
            moneyOrVolume = req.moneyOrVolume;
            if (req.CommMode != null)
                CommMode = Convert.ToInt32(req.CommMode);
            if (req.LinkMode != null)
                LinkMode = Convert.ToInt32(req.LinkMode);
            if (req.deviceNo != null)
                deviceNo = StringHelper.Instance.GetLastStr(req.deviceNo, 15);
            //if (req.lat != null)表的经纬度不是这里更新的，不要走这-txy
            //    lat = req.lat;
            //if (req.lng != null)
            //    lng = req.lng;
            if (req.meterNo != null)
                meterNo = Convert.ToInt32(req.meterNo);
            if (req.communicateNo != null)
                communicateNo = req.communicateNo;
            //if (req.barCode != null)
            //    barCode = req.barCode;
            if (req.meterTypeNo != null)
                meterTypeNo = req.meterTypeNo;
            if (req.factoryNo != null)
                factoryNo = req.factoryNo;
            if (req.openState != null)
                openState = Convert.ToInt32(req.openState);
            if (req.caliber != null)
                caliber = req.caliber;
            //if (req.baseVolume != null)
            //    baseVolume = Convert.ToDecimal(req.baseVolume);
            if (req.fluidNo != null)
                fluidNo = req.fluidNo;
            if (req.remark != null)
                remark = req.remark;
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            //if (req.collectorNo != null)
            //    collectorNo = req.collectorNo;
            if (req.AlarmTimes != null)
                AlarmTimes = Convert.ToInt32(req.AlarmTimes);
            if (req.AlarmInvTime != null)
                AlarmInvTime = Convert.ToInt32(req.AlarmInvTime);
            if (req.TempUpper != null)
                TempUpper = Convert.ToDecimal(req.TempUpper);
            if (req.TempLow != null)
                TempLow = Convert.ToDecimal(req.TempLow);
            if (req.PressUpper != null)
                PressUpper = Convert.ToDecimal(req.PressUpper);
            if (req.PressLow != null)
                PressLow = Convert.ToDecimal(req.PressLow);
            if (req.StdFlowUpper != null)
                StdFlowUpper = Convert.ToDecimal(req.StdFlowUpper);
            if (req.StdFlowLow != null)
                StdFlowLow = Convert.ToDecimal(req.StdFlowLow);
            if (req.WorkFlowUpper != null)
                WorkFlowUpper = Convert.ToDecimal(req.WorkFlowUpper);
            if (req.WorkFlowLow != null)
                WorkFlowLow = Convert.ToDecimal(req.WorkFlowLow);
            if (req.RemainMoneyLow != null)
                RemainMoneyLow = Convert.ToDecimal(req.RemainMoneyLow);
            if (req.RemainVolumLow != null)
                RemainVolumLow = Convert.ToDecimal(req.RemainVolumLow);
            if (req.OverMoneyUpper != null)
                OverMoneyUpper = Convert.ToDecimal(req.OverMoneyUpper);
            if (req.OverVolumeUpper != null)
                OverVolumeUpper = Convert.ToDecimal(req.OverVolumeUpper);
            if (req.DoorAlarm != null)
                DoorAlarm = Convert.ToInt32(req.DoorAlarm);

            if (req.BatteryLow != null)
                BatteryLow = Convert.ToDecimal(req.BatteryLow);
            if (req.ProtocolNo != null)
                ProtocolNo = req.ProtocolNo;
            if (req.IsIC != null)
                IsIC = req.IsIC;
        }

        public DeviceInfo(UpdateDeviceAlarmConfigReq req)
        {
            meterNo = Convert.ToInt32(req.meterNo);
            if (req.TempUpper != null)
                TempUpper = Convert.ToDecimal(req.TempUpper);
            if (req.TempLow != null)
                TempLow = Convert.ToDecimal(req.TempLow);
            if (req.PressUpper != null)
                PressUpper = Convert.ToDecimal(req.PressUpper);
            if (req.PressLow != null)
                PressLow = Convert.ToDecimal(req.PressLow);
            if (req.StdFlowUpper != null)
                StdFlowUpper = Convert.ToDecimal(req.StdFlowUpper);
            if (req.StdFlowLow != null)
                StdFlowLow = Convert.ToDecimal(req.StdFlowLow);
            if (req.WorkFlowUpper != null)
                WorkFlowUpper = Convert.ToDecimal(req.WorkFlowUpper);
            if (req.WorkFlowLow != null)
                WorkFlowLow = Convert.ToDecimal(req.WorkFlowLow);
            if (req.RemainMoneyLow != null)
                RemainMoneyLow = Convert.ToDecimal(req.RemainMoneyLow);
            if (req.RemainVolumLow != null)
                RemainVolumLow = Convert.ToDecimal(req.RemainVolumLow);
            if (req.OverMoneyUpper != null)
                OverMoneyUpper = Convert.ToDecimal(req.OverMoneyUpper);
            if (req.OverVolumeUpper != null)
                OverVolumeUpper = Convert.ToDecimal(req.OverVolumeUpper);
            if (req.DoorAlarm != null)
                DoorAlarm = Convert.ToInt32(req.DoorAlarm);
            if (req.PowerUpper != null)
                PowerUpper = Convert.ToDecimal(req.PowerUpper);
            if (req.PowerLow != null)
                PowerLow = Convert.ToDecimal(req.PowerLow);
            if (req.BatteryLow != null)
                BatteryLow = Convert.ToDecimal(req.BatteryLow);
        }

    }
}
