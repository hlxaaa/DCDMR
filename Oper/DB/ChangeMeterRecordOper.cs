using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class ChangeMeterRecordOper : SingleTon<ChangeMeterRecordOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="changemeterrecord"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(ChangeMeterRecord changemeterrecord)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (changemeterrecord.Id != null)
                dict.Add("@Id", changemeterrecord.Id.ToString());
            if (changemeterrecord.customerNo != null)
                dict.Add("@customerNo", changemeterrecord.customerNo.ToString());
            if (changemeterrecord.oldMeterNo != null)
                dict.Add("@oldMeterNo", changemeterrecord.oldMeterNo.ToString());
            if (changemeterrecord.oldCommunicateNo != null)
                dict.Add("@oldCommunicateNo", changemeterrecord.oldCommunicateNo.ToString());
            if (changemeterrecord.oldCumulant != null)
                dict.Add("@oldCumulant", changemeterrecord.oldCumulant.ToString());
            if (changemeterrecord.oldSurplus != null)
                dict.Add("@oldSurplus", changemeterrecord.oldSurplus.ToString());
            if (changemeterrecord.oldOverDraft != null)
                dict.Add("@oldOverDraft", changemeterrecord.oldOverDraft.ToString());
            if (changemeterrecord.oldCaliber != null)
                dict.Add("@oldCaliber", changemeterrecord.oldCaliber.ToString());
            if (changemeterrecord.oldFluidNo != null)
                dict.Add("@oldFluidNo", changemeterrecord.oldFluidNo.ToString());
            if (changemeterrecord.oldBaseVolume != null)
                dict.Add("@oldBaseVolume", changemeterrecord.oldBaseVolume.ToString());
            if (changemeterrecord.newMeterNo != null)
                dict.Add("@newMeterNo", changemeterrecord.newMeterNo.ToString());
            if (changemeterrecord.newCommunicateNo != null)
                dict.Add("@newCommunicateNo", changemeterrecord.newCommunicateNo.ToString());
            if (changemeterrecord.newBaseVolume != null)
                dict.Add("@newBaseVolume", changemeterrecord.newBaseVolume.ToString());
            if (changemeterrecord.newFluidNo != null)
                dict.Add("@newFluidNo", changemeterrecord.newFluidNo.ToString());
            if (changemeterrecord.newCaliber != null)
                dict.Add("@newCaliber", changemeterrecord.newCaliber.ToString());
            if (changemeterrecord.changeMan != null)
                dict.Add("@changeMan", changemeterrecord.changeMan.ToString());
            if (changemeterrecord.changeTime != null)
                dict.Add("@changeTime", changemeterrecord.changeTime.ToString());
            if (changemeterrecord.remark != null)
                dict.Add("@remark", changemeterrecord.remark.ToString());
            if (changemeterrecord.Operator != null)
                dict.Add("@Operator", changemeterrecord.Operator.ToString());
            if (changemeterrecord.operatorTime != null)
                dict.Add("@operatorTime", changemeterrecord.operatorTime.ToString());
            if (changemeterrecord.dispatchID != null)
                dict.Add("@dispatchID", changemeterrecord.dispatchID.ToString());
            if (changemeterrecord.oldRemainVolume != null)
                dict.Add("@oldRemainVolume", changemeterrecord.oldRemainVolume.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="changemeterrecord"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(ChangeMeterRecord changemeterrecord)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (changemeterrecord.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (changemeterrecord.oldMeterNo != null)
            {
                part1.Append("oldMeterNo,");
                part2.Append("@oldMeterNo,");
            }
            if (changemeterrecord.oldCommunicateNo != null)
            {
                part1.Append("oldCommunicateNo,");
                part2.Append("@oldCommunicateNo,");
            }
            if (changemeterrecord.oldCumulant != null)
            {
                part1.Append("oldCumulant,");
                part2.Append("@oldCumulant,");
            }
            if (changemeterrecord.oldSurplus != null)
            {
                part1.Append("oldSurplus,");
                part2.Append("@oldSurplus,");
            }
            if (changemeterrecord.oldOverDraft != null)
            {
                part1.Append("oldOverDraft,");
                part2.Append("@oldOverDraft,");
            }
            if (changemeterrecord.oldCaliber != null)
            {
                part1.Append("oldCaliber,");
                part2.Append("@oldCaliber,");
            }
            if (changemeterrecord.oldFluidNo != null)
            {
                part1.Append("oldFluidNo,");
                part2.Append("@oldFluidNo,");
            }
            if (changemeterrecord.oldBaseVolume != null)
            {
                part1.Append("oldBaseVolume,");
                part2.Append("@oldBaseVolume,");
            }
            if (changemeterrecord.newMeterNo != null)
            {
                part1.Append("newMeterNo,");
                part2.Append("@newMeterNo,");
            }
            if (changemeterrecord.newCommunicateNo != null)
            {
                part1.Append("newCommunicateNo,");
                part2.Append("@newCommunicateNo,");
            }
            if (changemeterrecord.newBaseVolume != null)
            {
                part1.Append("newBaseVolume,");
                part2.Append("@newBaseVolume,");
            }
            if (changemeterrecord.newFluidNo != null)
            {
                part1.Append("newFluidNo,");
                part2.Append("@newFluidNo,");
            }
            if (changemeterrecord.newCaliber != null)
            {
                part1.Append("newCaliber,");
                part2.Append("@newCaliber,");
            }
            if (changemeterrecord.changeMan != null)
            {
                part1.Append("changeMan,");
                part2.Append("@changeMan,");
            }
            if (changemeterrecord.changeTime != null)
            {
                part1.Append("changeTime,");
                part2.Append("@changeTime,");
            }
            if (changemeterrecord.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            if (changemeterrecord.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (changemeterrecord.operatorTime != null)
            {
                part1.Append("operatorTime,");
                part2.Append("@operatorTime,");
            }
            if (changemeterrecord.dispatchID != null)
            {
                part1.Append("dispatchID,");
                part2.Append("@dispatchID,");
            }
            if (changemeterrecord.oldRemainVolume != null)
            {
                part1.Append("oldRemainVolume,");
                part2.Append("@oldRemainVolume,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into changemeterrecord(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="changemeterrecord"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(ChangeMeterRecord changemeterrecord)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update changemeterrecord set ");
            if (changemeterrecord.customerNo != null)
                part1.Append("customerNo = @customerNo,");
            if (changemeterrecord.oldMeterNo != null)
                part1.Append("oldMeterNo = @oldMeterNo,");
            if (changemeterrecord.oldCommunicateNo != null)
                part1.Append("oldCommunicateNo = @oldCommunicateNo,");
            if (changemeterrecord.oldCumulant != null)
                part1.Append("oldCumulant = @oldCumulant,");
            if (changemeterrecord.oldSurplus != null)
                part1.Append("oldSurplus = @oldSurplus,");
            if (changemeterrecord.oldOverDraft != null)
                part1.Append("oldOverDraft = @oldOverDraft,");
            if (changemeterrecord.oldCaliber != null)
                part1.Append("oldCaliber = @oldCaliber,");
            if (changemeterrecord.oldFluidNo != null)
                part1.Append("oldFluidNo = @oldFluidNo,");
            if (changemeterrecord.oldBaseVolume != null)
                part1.Append("oldBaseVolume = @oldBaseVolume,");
            if (changemeterrecord.newMeterNo != null)
                part1.Append("newMeterNo = @newMeterNo,");
            if (changemeterrecord.newCommunicateNo != null)
                part1.Append("newCommunicateNo = @newCommunicateNo,");
            if (changemeterrecord.newBaseVolume != null)
                part1.Append("newBaseVolume = @newBaseVolume,");
            if (changemeterrecord.newFluidNo != null)
                part1.Append("newFluidNo = @newFluidNo,");
            if (changemeterrecord.newCaliber != null)
                part1.Append("newCaliber = @newCaliber,");
            if (changemeterrecord.changeMan != null)
                part1.Append("changeMan = @changeMan,");
            if (changemeterrecord.changeTime != null)
                part1.Append("changeTime = @changeTime,");
            if (changemeterrecord.remark != null)
                part1.Append("remark = @remark,");
            if (changemeterrecord.Operator != null)
                part1.Append("Operator = @Operator,");
            if (changemeterrecord.operatorTime != null)
                part1.Append("operatorTime = @operatorTime,");
            if (changemeterrecord.dispatchID != null)
                part1.Append("dispatchID = @dispatchID,");
            if (changemeterrecord.oldRemainVolume != null)
                part1.Append("oldRemainVolume = @oldRemainVolume,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="ChangeMeterRecord"></param>
        /// <returns></returns>
        public int Add(ChangeMeterRecord model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="ChangeMeterRecord"></param>
        /// <returns></returns>
        public void Update(ChangeMeterRecord model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
