using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_ReceiveDataOper : SingleTon<AllInOne_ReceiveDataOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_receivedata"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_ReceiveData allinone_receivedata)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_receivedata.Id != null)
                dict.Add("@Id", allinone_receivedata.Id.ToString());
            if (allinone_receivedata.CommNo != null)
                dict.Add("@CommNo", allinone_receivedata.CommNo.ToString());
            if (allinone_receivedata.MSTAISEQ != null)
                dict.Add("@MSTAISEQ", allinone_receivedata.MSTAISEQ.ToString());
            if (allinone_receivedata.MSTA != null)
                dict.Add("@MSTA", allinone_receivedata.MSTA.ToString());
            if (allinone_receivedata.ISEQ != null)
                dict.Add("@ISEQ", allinone_receivedata.ISEQ.ToString());
            if (allinone_receivedata.FSEQ != null)
                dict.Add("@FSEQ", allinone_receivedata.FSEQ.ToString());
            if (allinone_receivedata.ctrlCode != null)
                dict.Add("@ctrlCode", allinone_receivedata.ctrlCode.ToString());
            if (allinone_receivedata.sysType != null)
                dict.Add("@sysType", allinone_receivedata.sysType.ToString());
            if (allinone_receivedata.devType != null)
                dict.Add("@devType", allinone_receivedata.devType.ToString());
            if (allinone_receivedata.dataCmd != null)
                dict.Add("@dataCmd", allinone_receivedata.dataCmd.ToString());
            if (allinone_receivedata.dataArea != null)
                dict.Add("@dataArea", allinone_receivedata.dataArea.ToString());
            if (allinone_receivedata.ReceiveTime != null)
                dict.Add("@ReceiveTime", allinone_receivedata.ReceiveTime.ToString());
            if (allinone_receivedata.dealFlag != null)
                dict.Add("@dealFlag", allinone_receivedata.dealFlag.ToString());
            if (allinone_receivedata.dealTime != null)
                dict.Add("@dealTime", allinone_receivedata.dealTime.ToString());
            if (allinone_receivedata.dealMsg != null)
                dict.Add("@dealMsg", allinone_receivedata.dealMsg.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_receivedata"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_ReceiveData allinone_receivedata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_receivedata.CommNo != null)
            {
                part1.Append("CommNo,");
                part2.Append("@CommNo,");
            }
            if (allinone_receivedata.MSTAISEQ != null)
            {
                part1.Append("MSTAISEQ,");
                part2.Append("@MSTAISEQ,");
            }
            if (allinone_receivedata.MSTA != null)
            {
                part1.Append("MSTA,");
                part2.Append("@MSTA,");
            }
            if (allinone_receivedata.ISEQ != null)
            {
                part1.Append("ISEQ,");
                part2.Append("@ISEQ,");
            }
            if (allinone_receivedata.FSEQ != null)
            {
                part1.Append("FSEQ,");
                part2.Append("@FSEQ,");
            }
            if (allinone_receivedata.ctrlCode != null)
            {
                part1.Append("ctrlCode,");
                part2.Append("@ctrlCode,");
            }
            if (allinone_receivedata.sysType != null)
            {
                part1.Append("sysType,");
                part2.Append("@sysType,");
            }
            if (allinone_receivedata.devType != null)
            {
                part1.Append("devType,");
                part2.Append("@devType,");
            }
            if (allinone_receivedata.dataCmd != null)
            {
                part1.Append("dataCmd,");
                part2.Append("@dataCmd,");
            }
            if (allinone_receivedata.dataArea != null)
            {
                part1.Append("dataArea,");
                part2.Append("@dataArea,");
            }
            if (allinone_receivedata.ReceiveTime != null)
            {
                part1.Append("ReceiveTime,");
                part2.Append("@ReceiveTime,");
            }
            if (allinone_receivedata.dealFlag != null)
            {
                part1.Append("dealFlag,");
                part2.Append("@dealFlag,");
            }
            if (allinone_receivedata.dealTime != null)
            {
                part1.Append("dealTime,");
                part2.Append("@dealTime,");
            }
            if (allinone_receivedata.dealMsg != null)
            {
                part1.Append("dealMsg,");
                part2.Append("@dealMsg,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_receivedata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_receivedata"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_ReceiveData allinone_receivedata)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_receivedata set ");
            if (allinone_receivedata.CommNo != null)
                part1.Append("CommNo = @CommNo,");
            if (allinone_receivedata.MSTAISEQ != null)
                part1.Append("MSTAISEQ = @MSTAISEQ,");
            if (allinone_receivedata.MSTA != null)
                part1.Append("MSTA = @MSTA,");
            if (allinone_receivedata.ISEQ != null)
                part1.Append("ISEQ = @ISEQ,");
            if (allinone_receivedata.FSEQ != null)
                part1.Append("FSEQ = @FSEQ,");
            if (allinone_receivedata.ctrlCode != null)
                part1.Append("ctrlCode = @ctrlCode,");
            if (allinone_receivedata.sysType != null)
                part1.Append("sysType = @sysType,");
            if (allinone_receivedata.devType != null)
                part1.Append("devType = @devType,");
            if (allinone_receivedata.dataCmd != null)
                part1.Append("dataCmd = @dataCmd,");
            if (allinone_receivedata.dataArea != null)
                part1.Append("dataArea = @dataArea,");
            if (allinone_receivedata.ReceiveTime != null)
                part1.Append("ReceiveTime = @ReceiveTime,");
            if (allinone_receivedata.dealFlag != null)
                part1.Append("dealFlag = @dealFlag,");
            if (allinone_receivedata.dealTime != null)
                part1.Append("dealTime = @dealTime,");
            if (allinone_receivedata.dealMsg != null)
                part1.Append("dealMsg = @dealMsg,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_ReceiveData"></param>
        /// <returns></returns>
        public int Add(AllInOne_ReceiveData model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_ReceiveData"></param>
        /// <returns></returns>
        public void Update(AllInOne_ReceiveData model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
