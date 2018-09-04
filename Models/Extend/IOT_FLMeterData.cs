using System;

namespace DbOpertion.Models
{
    public partial class IOT_FLMeterData
    {

        public IOT_FLMeterData(IOT0000000001 req)
        {
            if (req.Id != null)
                Id = Convert.ToInt32(req.Id);
            if (req.communicateNo != null)
                communicateNo = req.communicateNo;
            if (req.FLMeterNo != null)
                FLMeterNo = Convert.ToInt32(req.FLMeterNo);
            if (req.siteNo != null)
                siteNo = req.siteNo;
            if (req.InstantTime != null)
                InstantTime = Convert.ToDateTime(req.InstantTime);
            if (req.ReceivTime != null)
                ReceivTime = Convert.ToDateTime(req.ReceivTime);
            if (req.StdSum != null)
                StdSum = Convert.ToDecimal(req.StdSum);
            if (req.WorkSum != null)
                WorkSum = Convert.ToDecimal(req.WorkSum);
            if (req.StdFlow != null)
                StdFlow = Convert.ToDecimal(req.StdFlow);
            if (req.WorkFlow != null)
                WorkFlow = Convert.ToDecimal(req.WorkFlow);
            if (req.Temperature != null)
                Temperature = Convert.ToDecimal(req.Temperature);
            if (req.Pressure != null)
                Pressure = Convert.ToDecimal(req.Pressure);
            if (req.FMState != null)
                FMState = Convert.ToInt32(req.FMState);
            if (req.FMStateMsg != null)
                FMStateMsg = req.FMStateMsg;
            if (req.RTUState != null)
                RTUState = Convert.ToInt32(req.RTUState);
            if (req.RTUStateMsg != null)
                RTUStateMsg = req.RTUStateMsg;
            if (req.SumTotal != null)
                SumTotal = Convert.ToDecimal(req.SumTotal);
            if (req.RemainMoney != null)
                RemainMoney = Convert.ToDecimal(req.RemainMoney);
            if (req.RemainVolume != null)
                RemainVolume = Convert.ToDecimal(req.RemainVolume);
            if (req.Overdraft != null)
                Overdraft = Convert.ToDecimal(req.Overdraft);
            if (req.RemoteChargeMoney != null)
                RemoteChargeMoney = Convert.ToDecimal(req.RemoteChargeMoney);
            if (req.RemoteChargeTimes != null)
                RemoteChargeTimes = Convert.ToInt32(req.RemoteChargeTimes);
            if (req.Price != null)
                Price = Convert.ToDecimal(req.Price);
            if (req.ValveState != null)
                ValveState = Convert.ToInt32(req.ValveState);
            if (req.ValveStateMsg != null)
                ValveStateMsg = req.ValveStateMsg;
            if (req.PowerVoltage != null)
                PowerVoltage = Convert.ToDecimal(req.PowerVoltage);
            if (req.BatteryVoltage != null)
                BatteryVoltage = Convert.ToDecimal(req.BatteryVoltage);
            if (req.Reserve1 != null)
                Reserve1 = req.Reserve1;
            if (req.Reserve2 != null)
                Reserve2 = req.Reserve2;
            if (req.Reserve3 != null)
                Reserve3 = req.Reserve3;
            if (req.Reserve4 != null)
                Reserve4 = req.Reserve4;
        }

    }
}
