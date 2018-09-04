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
    public partial class AllInOne_Device_AreaOper : SingleTon<AllInOne_Device_AreaOper>
    {
        public AllInOne_Device_Area GetByMeterNo(int meterno)
        {
            var str = $"select * from AllInOne_Device_Area where deviceId={meterno}";
            var list = SqlHelper.Instance.ExecuteGetDt<AllInOne_Device_Area>(str, new Dictionary<string, string>());
            if (list.Count == 0)
                return null;
            return list.First();
        }

        /// <summary>
        /// 添加设备时，要添加设备所在区域
        /// </summary>
        /// <param name="user"></param>
        /// <param name="deviceId"></param>
        public void AddDeviceWithArea(AllInOne_UserInfo user, int deviceId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            //-txy 超级管理员添加设备不添加区域不记得是为什么。
            //if (user.level != 100)
            //{
            AllInOne_Device_Area ada = new AllInOne_Device_Area();
            ada.areaId = user.areaId;
            ada.deviceId = deviceId;
            Add(ada, conn, tran);
            //}
        }

        /// <summary>
        /// 子账号改变区域后，设备绑定区域的记录也要一起改变
        /// </summary>
        /// <param name="sAreaId"></param>
        /// <param name="newAreaId"></param>
        public void UpdateAreaId(int sAreaId, int newAreaId, SqlConnection conn, SqlTransaction tran)
        {
            string str = $"update AllInOne_Device_Area set areaId={newAreaId} where areaId={sAreaId}";
            SqlHelper.Instance.ExcuteNon(str, new Dictionary<string, string>(), conn, tran);
        }

    }
}
