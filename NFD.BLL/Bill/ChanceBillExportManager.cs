using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
namespace NFD.BLL.Bill
{
    public class ChanceBillExportManager
    {
        /// <summary>
        /// 导出报价单
        /// </summary>
        /// <param name="traderId">客户id</param>
        /// <param name="from">开始时间</param>
        /// <param name="to">结速时间，</param>
        /// <param name="priceType">价钱类型  1成本价 2市场价 3客户价</param>
        public static void Export(int traderId, DateTime from, DateTime to, int priceType, int typeId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                Aspose.Cells.Workbook workbook = CreateWork(traderId, db, from, to, priceType);

                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.Buffer = true;
                response.Charset = "utf-8";
                if (typeId == 0)
                {
                    response.AppendHeader("Content-Disposition", "attachment;filename=" + "报价单.xls");
                    response.ContentEncoding = System.Text.Encoding.UTF8;
                    response.ContentType = "application/ms-excel";
                    response.BinaryWrite(workbook.SaveToStream().ToArray());
                    response.End();
                }
                else
                {
                    response.ContentType = "application/pdf";
                    string fileName = FileHelper.GetMapPath("~/Content/File/Temp/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "报价单.pdf");
                    workbook.Save(fileName, SaveFormat.Pdf);
                    FileHelper.ShowPDF(fileName);
                }

            }

        }

        /// <summary>
        /// 保存报价单
        /// </summary>
        /// <param name="traderId">客户id</param>
        /// <param name="from">开始时间</param>
        /// <param name="to">结速时间，</param>
        /// <param name="priceType">价钱类型  1成本价 2市场价 3客户价</param>
        public static string SaveExport(int traderId, DateTime from, DateTime to, int priceType, int typeId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                Aspose.Cells.Workbook workbook = CreateWork(traderId, db, from, to, priceType);
                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.Buffer = true;
                response.Charset = "utf-8";
                response.ContentType = "application/pdf";
                string fileName = FileHelper.GetMapPath("~/Content/File/Temp/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "报价单.pdf");
                workbook.Save(fileName, SaveFormat.Pdf);
                return fileName;

            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="db"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="priceType">价钱类型  1成本价 2市场价 3客户价</param>
        /// <returns></returns>
        public static Aspose.Cells.Workbook CreateWork(int traderId, NFDEntities db, DateTime from, DateTime to, int priceType)
        {
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();

            var chanceList = db.V_ChanceBill.Where(c => c.trader_id == traderId).Where(c => c.create_time >= from && c.create_time <= to).OrderByDescending(c => c.bill_id).ToList();
            foreach (var r in chanceList)
            {
                var i = chanceList.IndexOf(r);
                Aspose.Cells.Worksheet cellSheet = null;
                if (i == 0)//使用默认sheet1
                {
                    cellSheet = workbook.Worksheets[i];
                    cellSheet.Name = "报价单" + (1 + i);
                }
                else
                {
                    workbook.Worksheets.Add("报价单" + (1 + i));//创建新sheet
                    cellSheet = workbook.Worksheets[i];
                }
                var cells = cellSheet.Cells;
                cells.SetColumnWidth(0, 20);
                cells.SetColumnWidth(1, 20);
                cells.SetColumnWidth(2, 20);
                cells.SetColumnWidth(3, 20);

                cells.Merge(0, 0, 1, 1);//合并单元格 
                cells[0, 0].PutValue("款号");//填写内容 
                cells[0, 0].SetStyle(AsposeExcel.GetStyle(workbook, 2));

                cells.SetRowHeight(0, 38);
                cells.Merge(0, 1, 1, 3);//合并单元格 
                cells[0, 1].PutValue(r.clothing_number);//填写内容 
                cells[0, 1].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[0, 2].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[0, 3].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells.SetRowHeight(0, 38);



                //生成行2 标题行    
                cells.Merge(1, 0, 1, 4);//合并单元格 
                cells[1, 0].PutValue("面料规格:" + r.ht_specifications+" 幅宽:"+r.door+" 成份:"+r.element);//填写内容 
                cells[1, 0].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[1, 1].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[1, 2].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[1, 3].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells.SetRowHeight(1, 38);


                //生成行3 标题行    
                cells[2, 0].PutValue("面料款号");//填写内容 
                cells[2, 1].PutValue("用料");//填写内容 
                cells[2, 2].PutValue("面料价");//填写内容 
                cells[2, 3].PutValue("小计");//填写内容 
                cells[1, 0].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[1, 1].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[1, 2].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[1, 3].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells.SetRowHeight(2, 38);


                //生成行4 标题行    
                cells[2, 0].PutValue("面料款号");//填写内容 
                cells[2, 1].PutValue("用料");//填写内容 
                cells[2, 2].PutValue("面料价");//填写内容 
                cells[2, 3].PutValue("小计");//填写内容 
                cells[2, 0].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[2, 1].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[2, 2].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[2, 3].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells.SetRowHeight(2, 38);


                //生成行4 标题行    
                cells[3, 0].PutValue(r.ht_no);//填写内容 
                cells[3, 1].PutValue(r.num);//填写内容 
                if (priceType == 1)
                {
                    cells[3, 2].PutValue(r.cost_price.ToDecimal("n2"));//填写内容 
                    cells[3, 3].PutValue((r.num * r.cost_price).ToDecimal("n2"));//填写内容 
                }
                else if (priceType == 2)
                {
                    cells[3, 2].PutValue(r.market_price.ToDecimal("n2"));//填写内容 
                    cells[3, 3].PutValue((r.num * r.market_price).ToDecimal("n2"));//填写内容 
                }
                else
                {
                    cells[3, 2].PutValue(r.custome_price.ToDecimal("n2"));//填写内容 
                    cells[3, 3].PutValue((r.num * r.custome_price).ToDecimal("n2"));//填写内容 
                }
                cells[3, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[3, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[3, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[3, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(3, 38);


                //生成行4 标题行    
                cells[4, 0].PutValue("检品费");//填写内容 
                cells[4, 3].PutValue(r.check_price);
                cells[4, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[4, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[4, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[4, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(4, 38);

                //生成行5 标题行    
                cells[5, 0].PutValue("加工费");//填写内容 
                cells[5, 3].PutValue(r.pricessing_fee);//填写内容 
                cells[5, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[5, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[5, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[5, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(5, 38);


                //生成行6 标题行    
                cells[6, 0].PutValue("辅料");//填写内容 
                cells[6, 1].PutValue("用料");//填写内容 
                cells[6, 2].PutValue("单价");//填写内容 
                cells[6, 3].PutValue("小计");//填写内容 
                cells[6, 0].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[6, 1].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[6, 2].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells[6, 3].SetStyle(AsposeExcel.GetStyle(workbook, 2));
                cells.SetRowHeight(6, 38);

                int j = 7;
                var detailList = db.ChanceBillDetail.Where(c => c.bill_id == r.bill_id).ToList();
                foreach (var rr in detailList)
                {
                    j = j+1;
                    //生成行j 标题行    
                    cells[j, 0].PutValue(rr.name);//填写内容 
                    cells[j, 1].PutValue(rr.num.ToDecimal("n2"));//填写内容 
                    if (priceType == 1)
                    {
                        cells[j, 2].PutValue(rr.cost_price.ToDecimal("n2"));//填写内容 
                        cells[j, 3].PutValue((rr.num * rr.cost_price).ToDecimal("n2"));//填写内容 
                    }
                    else if (priceType == 2)
                    {
                        cells[j, 2].PutValue(rr.market_price.ToDecimal("n2"));//填写内容 
                        cells[j, 3].PutValue((rr.num * rr.market_price).ToDecimal("n2"));//填写内容 
                    }
                    else
                    {
                        cells[j, 2].PutValue(rr.custome_price.ToDecimal("n2"));//填写内容 
                        cells[j, 3].PutValue((rr.num * rr.custome_price).ToDecimal("n2"));//填写内容 
                    }
                    cells[j, 4].PutValue(rr.supplier_name);
                    cells[j, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                    cells[j, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                    cells[j, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                    cells[j, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                    cells.SetRowHeight(j, 38);

                }


                ++j;
                //生成行j 标题行    
                cells[j, 0].PutValue("TTL:（含损耗3%）");//填写内容 
                cells[j, 1].PutValue("");//填写内容 
               
                cells[j, 2].PutValue("");//填写内容 
                cells[j, 3].PutValue(detailList.Select(c =>( c.cost_price.ToDecimal() * c.num)*Convert.ToDecimal(1.03)).Sum().ToMoneyString());//填写内容 
              

                cells[j, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(j, 38);
                ++j; ++j;
                //生成行j 标题行    
                cells[j, 0].PutValue("运费通关费:");//填写内容 
                cells[j, 3].PutValue(r.postage.ToDecimal("n2"));//填写内容 
                cells[j, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(j, 38);

                j++;

                //生成行j 标题行    
                cells[j, 0].PutValue("合计：");//填写内容 
                
                 cells[j, 3].PutValue((r.detailCostPrice+r.check_price).ToDecimal("n2"));//填写内容 
                

                cells[j, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(j, 38);

                j++;
                //生成行j 标题行    
                cells[j, 0].PutValue("汇率:");//填写内容 
                cells[j, 3].PutValue(r.rate.ToDecimal("n2"));//填写内容 
                cells[j, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(j, 38);

                j++;
                //生成行j 标题行    
                cells[j, 0].PutValue("成衣价USD:");//填写内容 
               
                var tol=r.detailCostPrice +r.check_price;
                cells[j, 3].PutValue((tol / r.rate).ToDecimal("n2"));//填写内容 
              
              
                cells[j, 0].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 1].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 2].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells[j, 3].SetStyle(AsposeExcel.GetStyle(workbook, 3));
                cells.SetRowHeight(j, 38);


                j++;
                //生成行j 标题行    
                cells[j, 0].PutValue("备注：");//填写内容
                cells[j, 1].PutValue("纸型：" + r.paper);//填写内容
                cells.SetRowHeight(j, 38);
                cells.Merge(j,1,1,2);
                j++;
                //生成行j 标题行    
                cells[j, 2].PutValue(r.create_time.ToDateStr("yyyy-MM-dd"));//填写内容 
                cells.SetRowHeight(j, 38);

            }
            return workbook;
        }




    }
}
