using Common.Helper;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;

namespace HHTDCDMR.Controllers.Ajax
{
    public class RecordAjaxController : AjaxBaseController
    {
        /// <summary>
        /// 获取操作记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetOperRecordList(OperRecordReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetOperRecordList(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 上传excel过来，解析。-txy
        /// </summary>
        /// <returns></returns>
        public string GetExcelFile()
        {
            var f = Request.Files[0];
            var s = f.InputStream;
            HSSFWorkbook book = new HSSFWorkbook(s);
            var sheet = book.GetSheetAt(0);
            var dt = FileHelper.Instance.ExportToDataTable(sheet);
            var b = dt.Rows[0][0];
            var d = dt.Rows[15][1];
            var c = dt.Rows;
            //建立空白工作簿
            //IWorkbook workbook = new HSSFWorkbook();
            //在工作簿中：建立空白工作表
            //ISheet sheet = workbook.CreateSheet("情况表（1）");
            //workbook = book;
            //var a = 1;
            //创建Excel文件的对象
            //NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(Request.Files);

            //book = Request.Files;

            //string fileName = httpFile[0].FileName;
            //string newext = fileName.Substring(fileName.LastIndexOf("."));
            //string url = "/current/images/temp/" + RandHelper.Instance.Str(6) + DateTime.Now.ToString("yyyyMMddHHmmss") + newext;
            //httpFile[0].SaveAs(Server.MapPath(url));
            return "";
        }
    }
}