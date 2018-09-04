using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class DeviceInfoOper : SingleTon<DeviceInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="deviceinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(DeviceInfo deviceinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (deviceinfo.meterNo != null)
                dict.Add("@meterNo", deviceinfo.meterNo.ToString());
            if (deviceinfo.siteNo != null)
                dict.Add("@siteNo", deviceinfo.siteNo.ToString());
            if (deviceinfo.communicateNo != null)
                dict.Add("@communicateNo", deviceinfo.communicateNo.ToString());
            if (deviceinfo.CommAddr != null)
                dict.Add("@CommAddr", deviceinfo.CommAddr.ToString());
            if (deviceinfo.ProtocolNo != null)
                dict.Add("@ProtocolNo", deviceinfo.ProtocolNo.ToString());
            if (deviceinfo.CommMode != null)
                dict.Add("@CommMode", deviceinfo.CommMode.ToString());
            if (deviceinfo.LinkMode != null)
                dict.Add("@LinkMode", deviceinfo.LinkMode.ToString());
            if (deviceinfo.barCode != null)
                dict.Add("@barCode", deviceinfo.barCode.ToString());
            if (deviceinfo.customerNo != null)
                dict.Add("@customerNo", deviceinfo.customerNo.ToString());
            if (deviceinfo.meterTypeNo != null)
                dict.Add("@meterTypeNo", deviceinfo.meterTypeNo.ToString());
            if (deviceinfo.factoryNo != null)
                dict.Add("@factoryNo", deviceinfo.factoryNo.ToString());
            if (deviceinfo.openState != null)
                dict.Add("@openState", deviceinfo.openState.ToString());
            if (deviceinfo.caliber != null)
                dict.Add("@caliber", deviceinfo.caliber.ToString());
            if (deviceinfo.baseVolume != null)
                dict.Add("@baseVolume", deviceinfo.baseVolume.ToString());
            if (deviceinfo.fluidNo != null)
                dict.Add("@fluidNo", deviceinfo.fluidNo.ToString());
            if (deviceinfo.remark != null)
                dict.Add("@remark", deviceinfo.remark.ToString());
            if (deviceinfo.defineNo1 != null)
                dict.Add("@defineNo1", deviceinfo.defineNo1.ToString());
            if (deviceinfo.defineNo2 != null)
                dict.Add("@defineNo2", deviceinfo.defineNo2.ToString());
            if (deviceinfo.defineNo3 != null)
                dict.Add("@defineNo3", deviceinfo.defineNo3.ToString());
            if (deviceinfo.buildTime != null)
                dict.Add("@buildTime", deviceinfo.buildTime.ToString());
            if (deviceinfo.editTime != null)
                dict.Add("@editTime", deviceinfo.editTime.ToString());
            if (deviceinfo.Operator != null)
                dict.Add("@Operator", deviceinfo.Operator.ToString());
            if (deviceinfo.isConcentrate != null)
                dict.Add("@isConcentrate", deviceinfo.isConcentrate.ToString());
            if (deviceinfo.collectorNo != null)
                dict.Add("@collectorNo", deviceinfo.collectorNo.ToString());
            if (deviceinfo.MeterType != null)
                dict.Add("@MeterType", deviceinfo.MeterType.ToString());
            if (deviceinfo.Volatility != null)
                dict.Add("@Volatility", deviceinfo.Volatility.ToString());
            if (deviceinfo.AlarmTimes != null)
                dict.Add("@AlarmTimes", deviceinfo.AlarmTimes.ToString());
            if (deviceinfo.AlarmInvTime != null)
                dict.Add("@AlarmInvTime", deviceinfo.AlarmInvTime.ToString());
            if (deviceinfo.TempUpper != null)
                dict.Add("@TempUpper", deviceinfo.TempUpper.ToString());
            if (deviceinfo.TempLow != null)
                dict.Add("@TempLow", deviceinfo.TempLow.ToString());
            if (deviceinfo.PressUpper != null)
                dict.Add("@PressUpper", deviceinfo.PressUpper.ToString());
            if (deviceinfo.PressLow != null)
                dict.Add("@PressLow", deviceinfo.PressLow.ToString());
            if (deviceinfo.StdFlowUpper != null)
                dict.Add("@StdFlowUpper", deviceinfo.StdFlowUpper.ToString());
            if (deviceinfo.StdFlowLow != null)
                dict.Add("@StdFlowLow", deviceinfo.StdFlowLow.ToString());
            if (deviceinfo.WorkFlowUpper != null)
                dict.Add("@WorkFlowUpper", deviceinfo.WorkFlowUpper.ToString());
            if (deviceinfo.WorkFlowLow != null)
                dict.Add("@WorkFlowLow", deviceinfo.WorkFlowLow.ToString());
            if (deviceinfo.RemainMoneyLow != null)
                dict.Add("@RemainMoneyLow", deviceinfo.RemainMoneyLow.ToString());
            if (deviceinfo.RemainVolumLow != null)
                dict.Add("@RemainVolumLow", deviceinfo.RemainVolumLow.ToString());
            if (deviceinfo.OverMoneyUpper != null)
                dict.Add("@OverMoneyUpper", deviceinfo.OverMoneyUpper.ToString());
            if (deviceinfo.OverVolumeUpper != null)
                dict.Add("@OverVolumeUpper", deviceinfo.OverVolumeUpper.ToString());
            if (deviceinfo.DoorAlarm != null)
                dict.Add("@DoorAlarm", deviceinfo.DoorAlarm.ToString());
            if (deviceinfo.PowerUpper != null)
                dict.Add("@PowerUpper", deviceinfo.PowerUpper.ToString());
            if (deviceinfo.PowerLow != null)
                dict.Add("@PowerLow", deviceinfo.PowerLow.ToString());
            if (deviceinfo.BatteryLow != null)
                dict.Add("@BatteryLow", deviceinfo.BatteryLow.ToString());
            if (deviceinfo.Image != null)
                dict.Add("@Image", deviceinfo.Image.ToString());
            if (deviceinfo.IsValve != null)
                dict.Add("@IsValve", deviceinfo.IsValve.ToString());
            if (deviceinfo.DayFmStart != null)
                dict.Add("@DayFmStart", deviceinfo.DayFmStart.ToString());
            if (deviceinfo.lat != null)
                dict.Add("@lat", deviceinfo.lat.ToString());
            if (deviceinfo.lng != null)
                dict.Add("@lng", deviceinfo.lng.ToString());
            if (deviceinfo.deviceNo != null)
                dict.Add("@deviceNo", deviceinfo.deviceNo.ToString());
            if (deviceinfo.fluidType != null)
                dict.Add("@fluidType", deviceinfo.fluidType.ToString());
            if (deviceinfo.IsIC != null)
                dict.Add("@IsIC", deviceinfo.IsIC.ToString());
            if (deviceinfo.ScadaInvTime != null)
                dict.Add("@ScadaInvTime", deviceinfo.ScadaInvTime.ToString());
            if (deviceinfo.isEncrypt != null)
                dict.Add("@isEncrypt", deviceinfo.isEncrypt.ToString());
            if (deviceinfo.moneyOrVolume != null)
                dict.Add("@moneyOrVolume", deviceinfo.moneyOrVolume.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="deviceinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(DeviceInfo deviceinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (deviceinfo.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (deviceinfo.communicateNo != null)
            {
                part1.Append("communicateNo,");
                part2.Append("@communicateNo,");
            }
            if (deviceinfo.CommAddr != null)
            {
                part1.Append("CommAddr,");
                part2.Append("@CommAddr,");
            }
            if (deviceinfo.ProtocolNo != null)
            {
                part1.Append("ProtocolNo,");
                part2.Append("@ProtocolNo,");
            }
            if (deviceinfo.CommMode != null)
            {
                part1.Append("CommMode,");
                part2.Append("@CommMode,");
            }
            if (deviceinfo.LinkMode != null)
            {
                part1.Append("LinkMode,");
                part2.Append("@LinkMode,");
            }
            if (deviceinfo.barCode != null)
            {
                part1.Append("barCode,");
                part2.Append("@barCode,");
            }
            if (deviceinfo.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (deviceinfo.meterTypeNo != null)
            {
                part1.Append("meterTypeNo,");
                part2.Append("@meterTypeNo,");
            }
            if (deviceinfo.factoryNo != null)
            {
                part1.Append("factoryNo,");
                part2.Append("@factoryNo,");
            }
            if (deviceinfo.openState != null)
            {
                part1.Append("openState,");
                part2.Append("@openState,");
            }
            if (deviceinfo.caliber != null)
            {
                part1.Append("caliber,");
                part2.Append("@caliber,");
            }
            if (deviceinfo.baseVolume != null)
            {
                part1.Append("baseVolume,");
                part2.Append("@baseVolume,");
            }
            if (deviceinfo.fluidNo != null)
            {
                part1.Append("fluidNo,");
                part2.Append("@fluidNo,");
            }
            if (deviceinfo.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            if (deviceinfo.defineNo1 != null)
            {
                part1.Append("defineNo1,");
                part2.Append("@defineNo1,");
            }
            if (deviceinfo.defineNo2 != null)
            {
                part1.Append("defineNo2,");
                part2.Append("@defineNo2,");
            }
            if (deviceinfo.defineNo3 != null)
            {
                part1.Append("defineNo3,");
                part2.Append("@defineNo3,");
            }
            if (deviceinfo.buildTime != null)
            {
                part1.Append("buildTime,");
                part2.Append("@buildTime,");
            }
            if (deviceinfo.editTime != null)
            {
                part1.Append("editTime,");
                part2.Append("@editTime,");
            }
            if (deviceinfo.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (deviceinfo.isConcentrate != null)
            {
                part1.Append("isConcentrate,");
                part2.Append("@isConcentrate,");
            }
            if (deviceinfo.collectorNo != null)
            {
                part1.Append("collectorNo,");
                part2.Append("@collectorNo,");
            }
            if (deviceinfo.MeterType != null)
            {
                part1.Append("MeterType,");
                part2.Append("@MeterType,");
            }
            if (deviceinfo.Volatility != null)
            {
                part1.Append("Volatility,");
                part2.Append("@Volatility,");
            }
            if (deviceinfo.AlarmTimes != null)
            {
                part1.Append("AlarmTimes,");
                part2.Append("@AlarmTimes,");
            }
            if (deviceinfo.AlarmInvTime != null)
            {
                part1.Append("AlarmInvTime,");
                part2.Append("@AlarmInvTime,");
            }
            if (deviceinfo.TempUpper != null)
            {
                part1.Append("TempUpper,");
                part2.Append("@TempUpper,");
            }
            if (deviceinfo.TempLow != null)
            {
                part1.Append("TempLow,");
                part2.Append("@TempLow,");
            }
            if (deviceinfo.PressUpper != null)
            {
                part1.Append("PressUpper,");
                part2.Append("@PressUpper,");
            }
            if (deviceinfo.PressLow != null)
            {
                part1.Append("PressLow,");
                part2.Append("@PressLow,");
            }
            if (deviceinfo.StdFlowUpper != null)
            {
                part1.Append("StdFlowUpper,");
                part2.Append("@StdFlowUpper,");
            }
            if (deviceinfo.StdFlowLow != null)
            {
                part1.Append("StdFlowLow,");
                part2.Append("@StdFlowLow,");
            }
            if (deviceinfo.WorkFlowUpper != null)
            {
                part1.Append("WorkFlowUpper,");
                part2.Append("@WorkFlowUpper,");
            }
            if (deviceinfo.WorkFlowLow != null)
            {
                part1.Append("WorkFlowLow,");
                part2.Append("@WorkFlowLow,");
            }
            if (deviceinfo.RemainMoneyLow != null)
            {
                part1.Append("RemainMoneyLow,");
                part2.Append("@RemainMoneyLow,");
            }
            if (deviceinfo.RemainVolumLow != null)
            {
                part1.Append("RemainVolumLow,");
                part2.Append("@RemainVolumLow,");
            }
            if (deviceinfo.OverMoneyUpper != null)
            {
                part1.Append("OverMoneyUpper,");
                part2.Append("@OverMoneyUpper,");
            }
            if (deviceinfo.OverVolumeUpper != null)
            {
                part1.Append("OverVolumeUpper,");
                part2.Append("@OverVolumeUpper,");
            }
            if (deviceinfo.DoorAlarm != null)
            {
                part1.Append("DoorAlarm,");
                part2.Append("@DoorAlarm,");
            }
            if (deviceinfo.PowerUpper != null)
            {
                part1.Append("PowerUpper,");
                part2.Append("@PowerUpper,");
            }
            if (deviceinfo.PowerLow != null)
            {
                part1.Append("PowerLow,");
                part2.Append("@PowerLow,");
            }
            if (deviceinfo.BatteryLow != null)
            {
                part1.Append("BatteryLow,");
                part2.Append("@BatteryLow,");
            }
            if (deviceinfo.Image != null)
            {
                part1.Append("Image,");
                part2.Append("@Image,");
            }
            if (deviceinfo.IsValve != null)
            {
                part1.Append("IsValve,");
                part2.Append("@IsValve,");
            }
            if (deviceinfo.DayFmStart != null)
            {
                part1.Append("DayFmStart,");
                part2.Append("@DayFmStart,");
            }
            if (deviceinfo.lat != null)
            {
                part1.Append("lat,");
                part2.Append("@lat,");
            }
            if (deviceinfo.lng != null)
            {
                part1.Append("lng,");
                part2.Append("@lng,");
            }
            if (deviceinfo.deviceNo != null)
            {
                part1.Append("deviceNo,");
                part2.Append("@deviceNo,");
            }
            if (deviceinfo.fluidType != null)
            {
                part1.Append("fluidType,");
                part2.Append("@fluidType,");
            }
            if (deviceinfo.IsIC != null)
            {
                part1.Append("IsIC,");
                part2.Append("@IsIC,");
            }
            if (deviceinfo.ScadaInvTime != null)
            {
                part1.Append("ScadaInvTime,");
                part2.Append("@ScadaInvTime,");
            }
            if (deviceinfo.isEncrypt != null)
            {
                part1.Append("isEncrypt,");
                part2.Append("@isEncrypt,");
            }
            if (deviceinfo.moneyOrVolume != null)
            {
                part1.Append("moneyOrVolume,");
                part2.Append("@moneyOrVolume,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into deviceinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deviceinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(DeviceInfo deviceinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update deviceinfo set ");
            if (deviceinfo.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (deviceinfo.communicateNo != null)
                part1.Append("communicateNo = @communicateNo,");
            if (deviceinfo.CommAddr != null)
                part1.Append("CommAddr = @CommAddr,");
            if (deviceinfo.ProtocolNo != null)
                part1.Append("ProtocolNo = @ProtocolNo,");
            if (deviceinfo.CommMode != null)
                part1.Append("CommMode = @CommMode,");
            if (deviceinfo.LinkMode != null)
                part1.Append("LinkMode = @LinkMode,");
            if (deviceinfo.barCode != null)
                part1.Append("barCode = @barCode,");
            if (deviceinfo.customerNo != null)
                part1.Append("customerNo = @customerNo,");
            if (deviceinfo.meterTypeNo != null)
                part1.Append("meterTypeNo = @meterTypeNo,");
            if (deviceinfo.factoryNo != null)
                part1.Append("factoryNo = @factoryNo,");
            if (deviceinfo.openState != null)
                part1.Append("openState = @openState,");
            if (deviceinfo.caliber != null)
                part1.Append("caliber = @caliber,");
            if (deviceinfo.baseVolume != null)
                part1.Append("baseVolume = @baseVolume,");
            if (deviceinfo.fluidNo != null)
                part1.Append("fluidNo = @fluidNo,");
            if (deviceinfo.remark != null)
                part1.Append("remark = @remark,");
            if (deviceinfo.defineNo1 != null)
                part1.Append("defineNo1 = @defineNo1,");
            if (deviceinfo.defineNo2 != null)
                part1.Append("defineNo2 = @defineNo2,");
            if (deviceinfo.defineNo3 != null)
                part1.Append("defineNo3 = @defineNo3,");
            if (deviceinfo.buildTime != null)
                part1.Append("buildTime = @buildTime,");
            if (deviceinfo.editTime != null)
                part1.Append("editTime = @editTime,");
            if (deviceinfo.Operator != null)
                part1.Append("Operator = @Operator,");
            if (deviceinfo.isConcentrate != null)
                part1.Append("isConcentrate = @isConcentrate,");
            if (deviceinfo.collectorNo != null)
                part1.Append("collectorNo = @collectorNo,");
            if (deviceinfo.MeterType != null)
                part1.Append("MeterType = @MeterType,");
            if (deviceinfo.Volatility != null)
                part1.Append("Volatility = @Volatility,");
            if (deviceinfo.AlarmTimes != null)
                part1.Append("AlarmTimes = @AlarmTimes,");
            if (deviceinfo.AlarmInvTime != null)
                part1.Append("AlarmInvTime = @AlarmInvTime,");
            if (deviceinfo.TempUpper != null)
                part1.Append("TempUpper = @TempUpper,");
            if (deviceinfo.TempLow != null)
                part1.Append("TempLow = @TempLow,");
            if (deviceinfo.PressUpper != null)
                part1.Append("PressUpper = @PressUpper,");
            if (deviceinfo.PressLow != null)
                part1.Append("PressLow = @PressLow,");
            if (deviceinfo.StdFlowUpper != null)
                part1.Append("StdFlowUpper = @StdFlowUpper,");
            if (deviceinfo.StdFlowLow != null)
                part1.Append("StdFlowLow = @StdFlowLow,");
            if (deviceinfo.WorkFlowUpper != null)
                part1.Append("WorkFlowUpper = @WorkFlowUpper,");
            if (deviceinfo.WorkFlowLow != null)
                part1.Append("WorkFlowLow = @WorkFlowLow,");
            if (deviceinfo.RemainMoneyLow != null)
                part1.Append("RemainMoneyLow = @RemainMoneyLow,");
            if (deviceinfo.RemainVolumLow != null)
                part1.Append("RemainVolumLow = @RemainVolumLow,");
            if (deviceinfo.OverMoneyUpper != null)
                part1.Append("OverMoneyUpper = @OverMoneyUpper,");
            if (deviceinfo.OverVolumeUpper != null)
                part1.Append("OverVolumeUpper = @OverVolumeUpper,");
            if (deviceinfo.DoorAlarm != null)
                part1.Append("DoorAlarm = @DoorAlarm,");
            if (deviceinfo.PowerUpper != null)
                part1.Append("PowerUpper = @PowerUpper,");
            if (deviceinfo.PowerLow != null)
                part1.Append("PowerLow = @PowerLow,");
            if (deviceinfo.BatteryLow != null)
                part1.Append("BatteryLow = @BatteryLow,");
            if (deviceinfo.Image != null)
                part1.Append("Image = @Image,");
            if (deviceinfo.IsValve != null)
                part1.Append("IsValve = @IsValve,");
            if (deviceinfo.DayFmStart != null)
                part1.Append("DayFmStart = @DayFmStart,");
            if (deviceinfo.lat != null)
                part1.Append("lat = @lat,");
            if (deviceinfo.lng != null)
                part1.Append("lng = @lng,");
            if (deviceinfo.deviceNo != null)
                part1.Append("deviceNo = @deviceNo,");
            if (deviceinfo.fluidType != null)
                part1.Append("fluidType = @fluidType,");
            if (deviceinfo.IsIC != null)
                part1.Append("IsIC = @IsIC,");
            if (deviceinfo.ScadaInvTime != null)
                part1.Append("ScadaInvTime = @ScadaInvTime,");
            if (deviceinfo.isEncrypt != null)
                part1.Append("isEncrypt = @isEncrypt,");
            if (deviceinfo.moneyOrVolume != null)
                part1.Append("moneyOrVolume = @moneyOrVolume,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where meterNo= @meterNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="DeviceInfo"></param>
        /// <returns></returns>
        public int Add(DeviceInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="DeviceInfo"></param>
        /// <returns></returns>
        public int Update(DeviceInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            return SqlHelper.Instance.ExcuteNonQuery(str, dict, connection, transaction);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="deviceinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParametersItem(DeviceInfo deviceinfo,int i)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (deviceinfo.meterNo != null)
                dict.Add("@meterNo" + i, deviceinfo.meterNo.ToString());
            if (deviceinfo.siteNo != null)
                dict.Add("@siteNo" + i, deviceinfo.siteNo.ToString());
            if (deviceinfo.communicateNo != null)
                dict.Add("@communicateNo" + i, deviceinfo.communicateNo.ToString());
            if (deviceinfo.CommAddr != null)
                dict.Add("@CommAddr" + i, deviceinfo.CommAddr.ToString());
            if (deviceinfo.ProtocolNo != null)
                dict.Add("@ProtocolNo" + i, deviceinfo.ProtocolNo.ToString());
            if (deviceinfo.CommMode != null)
                dict.Add("@CommMode" + i, deviceinfo.CommMode.ToString());
            if (deviceinfo.LinkMode != null)
                dict.Add("@LinkMode" + i, deviceinfo.LinkMode.ToString());
            if (deviceinfo.barCode != null)
                dict.Add("@barCode" + i, deviceinfo.barCode.ToString());
            if (deviceinfo.customerNo != null)
                dict.Add("@customerNo" + i, deviceinfo.customerNo.ToString());
            if (deviceinfo.meterTypeNo != null)
                dict.Add("@meterTypeNo" + i, deviceinfo.meterTypeNo.ToString());
            if (deviceinfo.factoryNo != null)
                dict.Add("@factoryNo" + i, deviceinfo.factoryNo.ToString());
            if (deviceinfo.openState != null)
                dict.Add("@openState" + i, deviceinfo.openState.ToString());
            if (deviceinfo.caliber != null)
                dict.Add("@caliber" + i, deviceinfo.caliber.ToString());
            if (deviceinfo.baseVolume != null)
                dict.Add("@baseVolume" + i, deviceinfo.baseVolume.ToString());
            if (deviceinfo.fluidNo != null)
                dict.Add("@fluidNo" + i, deviceinfo.fluidNo.ToString());
            if (deviceinfo.remark != null)
                dict.Add("@remark" + i, deviceinfo.remark.ToString());
            if (deviceinfo.defineNo1 != null)
                dict.Add("@defineNo1" + i, deviceinfo.defineNo1.ToString());
            if (deviceinfo.defineNo2 != null)
                dict.Add("@defineNo2" + i, deviceinfo.defineNo2.ToString());
            if (deviceinfo.defineNo3 != null)
                dict.Add("@defineNo3" + i, deviceinfo.defineNo3.ToString());
            if (deviceinfo.buildTime != null)
                dict.Add("@buildTime" + i, deviceinfo.buildTime.ToString());
            if (deviceinfo.editTime != null)
                dict.Add("@editTime" + i, deviceinfo.editTime.ToString());
            if (deviceinfo.Operator != null)
                dict.Add("@Operator" + i, deviceinfo.Operator.ToString());
            if (deviceinfo.isConcentrate != null)
                dict.Add("@isConcentrate" + i, deviceinfo.isConcentrate.ToString());
            if (deviceinfo.collectorNo != null)
                dict.Add("@collectorNo" + i, deviceinfo.collectorNo.ToString());
            if (deviceinfo.MeterType != null)
                dict.Add("@MeterType" + i, deviceinfo.MeterType.ToString());
            if (deviceinfo.Volatility != null)
                dict.Add("@Volatility" + i, deviceinfo.Volatility.ToString());
            if (deviceinfo.AlarmTimes != null)
                dict.Add("@AlarmTimes" + i, deviceinfo.AlarmTimes.ToString());
            if (deviceinfo.AlarmInvTime != null)
                dict.Add("@AlarmInvTime" + i, deviceinfo.AlarmInvTime.ToString());
            if (deviceinfo.TempUpper != null)
                dict.Add("@TempUpper" + i, deviceinfo.TempUpper.ToString());
            if (deviceinfo.TempLow != null)
                dict.Add("@TempLow" + i, deviceinfo.TempLow.ToString());
            if (deviceinfo.PressUpper != null)
                dict.Add("@PressUpper" + i, deviceinfo.PressUpper.ToString());
            if (deviceinfo.PressLow != null)
                dict.Add("@PressLow" + i, deviceinfo.PressLow.ToString());
            if (deviceinfo.StdFlowUpper != null)
                dict.Add("@StdFlowUpper" + i, deviceinfo.StdFlowUpper.ToString());
            if (deviceinfo.StdFlowLow != null)
                dict.Add("@StdFlowLow" + i, deviceinfo.StdFlowLow.ToString());
            if (deviceinfo.WorkFlowUpper != null)
                dict.Add("@WorkFlowUpper" + i, deviceinfo.WorkFlowUpper.ToString());
            if (deviceinfo.WorkFlowLow != null)
                dict.Add("@WorkFlowLow" + i, deviceinfo.WorkFlowLow.ToString());
            if (deviceinfo.RemainMoneyLow != null)
                dict.Add("@RemainMoneyLow" + i, deviceinfo.RemainMoneyLow.ToString());
            if (deviceinfo.RemainVolumLow != null)
                dict.Add("@RemainVolumLow" + i, deviceinfo.RemainVolumLow.ToString());
            if (deviceinfo.OverMoneyUpper != null)
                dict.Add("@OverMoneyUpper" + i, deviceinfo.OverMoneyUpper.ToString());
            if (deviceinfo.OverVolumeUpper != null)
                dict.Add("@OverVolumeUpper" + i, deviceinfo.OverVolumeUpper.ToString());
            if (deviceinfo.DoorAlarm != null)
                dict.Add("@DoorAlarm" + i, deviceinfo.DoorAlarm.ToString());
            if (deviceinfo.PowerUpper != null)
                dict.Add("@PowerUpper" + i, deviceinfo.PowerUpper.ToString());
            if (deviceinfo.PowerLow != null)
                dict.Add("@PowerLow" + i, deviceinfo.PowerLow.ToString());
            if (deviceinfo.BatteryLow != null)
                dict.Add("@BatteryLow" + i, deviceinfo.BatteryLow.ToString());
            if (deviceinfo.Image != null)
                dict.Add("@Image" + i, deviceinfo.Image.ToString());
            if (deviceinfo.IsValve != null)
                dict.Add("@IsValve" + i, deviceinfo.IsValve.ToString());
            if (deviceinfo.DayFmStart != null)
                dict.Add("@DayFmStart" + i, deviceinfo.DayFmStart.ToString());
            if (deviceinfo.lat != null)
                dict.Add("@lat" + i, deviceinfo.lat.ToString());
            if (deviceinfo.lng != null)
                dict.Add("@lng" + i, deviceinfo.lng.ToString());
            if (deviceinfo.deviceNo != null)
                dict.Add("@deviceNo" + i, deviceinfo.deviceNo.ToString());
            if (deviceinfo.fluidType != null)
                dict.Add("@fluidType" + i, deviceinfo.fluidType.ToString());
            if (deviceinfo.IsIC != null)
                dict.Add("@IsIC" + i, deviceinfo.IsIC.ToString());
            if (deviceinfo.ScadaInvTime != null)
                dict.Add("@ScadaInvTime" + i, deviceinfo.ScadaInvTime.ToString());
            if (deviceinfo.isEncrypt != null)
                dict.Add("@isEncrypt" + i, deviceinfo.isEncrypt.ToString());
            if (deviceinfo.moneyOrVolume != null)
                dict.Add("@moneyOrVolume" + i, deviceinfo.moneyOrVolume.ToString());

            return dict;
        }        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deviceinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStrItem(DeviceInfo deviceinfo, int i)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update deviceinfo set ");
            if (deviceinfo.siteNo != null)
                part1.Append($"siteNo = @siteNo{i},");
            if (deviceinfo.communicateNo != null)
                part1.Append($"communicateNo = @communicateNo{i},");
            if (deviceinfo.CommAddr != null)
                part1.Append($"CommAddr = @CommAddr{i},");
            if (deviceinfo.ProtocolNo != null)
                part1.Append($"ProtocolNo = @ProtocolNo{i},");
            if (deviceinfo.CommMode != null)
                part1.Append($"CommMode = @CommMode{i},");
            if (deviceinfo.LinkMode != null)
                part1.Append($"LinkMode = @LinkMode{i},");
            if (deviceinfo.barCode != null)
                part1.Append($"barCode = @barCode{i},");
            if (deviceinfo.customerNo != null)
                part1.Append($"customerNo = @customerNo{i},");
            if (deviceinfo.meterTypeNo != null)
                part1.Append($"meterTypeNo = @meterTypeNo{i},");
            if (deviceinfo.factoryNo != null)
                part1.Append($"factoryNo = @factoryNo{i},");
            if (deviceinfo.openState != null)
                part1.Append($"openState = @openState{i},");
            if (deviceinfo.caliber != null)
                part1.Append($"caliber = @caliber{i},");
            if (deviceinfo.baseVolume != null)
                part1.Append($"baseVolume = @baseVolume{i},");
            if (deviceinfo.fluidNo != null)
                part1.Append($"fluidNo = @fluidNo{i},");
            if (deviceinfo.remark != null)
                part1.Append($"remark = @remark{i},");
            if (deviceinfo.defineNo1 != null)
                part1.Append($"defineNo1 = @defineNo1{i},");
            if (deviceinfo.defineNo2 != null)
                part1.Append($"defineNo2 = @defineNo2{i},");
            if (deviceinfo.defineNo3 != null)
                part1.Append($"defineNo3 = @defineNo3{i},");
            if (deviceinfo.buildTime != null)
                part1.Append($"buildTime = @buildTime{i},");
            if (deviceinfo.editTime != null)
                part1.Append($"editTime = @editTime{i},");
            if (deviceinfo.Operator != null)
                part1.Append($"Operator = @Operator{i},");
            if (deviceinfo.isConcentrate != null)
                part1.Append($"isConcentrate = @isConcentrate{i},");
            if (deviceinfo.collectorNo != null)
                part1.Append($"collectorNo = @collectorNo{i},");
            if (deviceinfo.MeterType != null)
                part1.Append($"MeterType = @MeterType{i},");
            if (deviceinfo.Volatility != null)
                part1.Append($"Volatility = @Volatility{i},");
            if (deviceinfo.AlarmTimes != null)
                part1.Append($"AlarmTimes = @AlarmTimes{i},");
            if (deviceinfo.AlarmInvTime != null)
                part1.Append($"AlarmInvTime = @AlarmInvTime{i},");
            if (deviceinfo.TempUpper != null)
                part1.Append($"TempUpper = @TempUpper{i},");
            if (deviceinfo.TempLow != null)
                part1.Append($"TempLow = @TempLow{i},");
            if (deviceinfo.PressUpper != null)
                part1.Append($"PressUpper = @PressUpper{i},");
            if (deviceinfo.PressLow != null)
                part1.Append($"PressLow = @PressLow{i},");
            if (deviceinfo.StdFlowUpper != null)
                part1.Append($"StdFlowUpper = @StdFlowUpper{i},");
            if (deviceinfo.StdFlowLow != null)
                part1.Append($"StdFlowLow = @StdFlowLow{i},");
            if (deviceinfo.WorkFlowUpper != null)
                part1.Append($"WorkFlowUpper = @WorkFlowUpper{i},");
            if (deviceinfo.WorkFlowLow != null)
                part1.Append($"WorkFlowLow = @WorkFlowLow{i},");
            if (deviceinfo.RemainMoneyLow != null)
                part1.Append($"RemainMoneyLow = @RemainMoneyLow{i},");
            if (deviceinfo.RemainVolumLow != null)
                part1.Append($"RemainVolumLow = @RemainVolumLow{i},");
            if (deviceinfo.OverMoneyUpper != null)
                part1.Append($"OverMoneyUpper = @OverMoneyUpper{i},");
            if (deviceinfo.OverVolumeUpper != null)
                part1.Append($"OverVolumeUpper = @OverVolumeUpper{i},");
            if (deviceinfo.DoorAlarm != null)
                part1.Append($"DoorAlarm = @DoorAlarm{i},");
            if (deviceinfo.PowerUpper != null)
                part1.Append($"PowerUpper = @PowerUpper{i},");
            if (deviceinfo.PowerLow != null)
                part1.Append($"PowerLow = @PowerLow{i},");
            if (deviceinfo.BatteryLow != null)
                part1.Append($"BatteryLow = @BatteryLow{i},");
            if (deviceinfo.Image != null)
                part1.Append($"Image = @Image{i},");
            if (deviceinfo.IsValve != null)
                part1.Append($"IsValve = @IsValve{i},");
            if (deviceinfo.DayFmStart != null)
                part1.Append($"DayFmStart = @DayFmStart{i},");
            if (deviceinfo.lat != null)
                part1.Append($"lat = @lat{i},");
            if (deviceinfo.lng != null)
                part1.Append($"lng = @lng{i},");
            if (deviceinfo.deviceNo != null)
                part1.Append($"deviceNo = @deviceNo{i},");
            if (deviceinfo.fluidType != null)
                part1.Append($"fluidType = @fluidType{i},");
            if (deviceinfo.IsIC != null)
                part1.Append($"IsIC = @IsIC{i},");
            if (deviceinfo.ScadaInvTime != null)
                part1.Append($"ScadaInvTime = @ScadaInvTime{i},");
            if (deviceinfo.isEncrypt != null)
                part1.Append($"isEncrypt = @isEncrypt{i},");
            if (deviceinfo.moneyOrVolume != null)
                part1.Append($"moneyOrVolume = @moneyOrVolume{i},");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append($" where meterNo= @meterNo{i}  ");
            return part1.ToString();
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="DeviceInfo"></param>
        /// <returns></returns>
        public void UpdateList(List<DeviceInfo> list, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = "";
            var dict = new Dictionary<string,string>();
            for(int i=0;i<list.Count;i++)
            {
            var tempDict=GetParametersItem(list[i],i);
            foreach(var item in tempDict)
            {
            dict.Add(item.Key,item.Value);
            }
            str+=GetUpdateStrItem(list[i],i);
            }
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
