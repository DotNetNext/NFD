using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Aspose.Cells;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Drawing;
namespace COM.Utility
{
    /// <summary>
    /// jailall.sun asposecell组件
    /// </summary>
    public class AsposeExcel
    {
        public class MergePar
        {
            public string Key { get; set; }
            public int MinColIndex { get; set; }
            public int MinRowIndex { get; set; }
            public int MaxColIndex { get; set; }
            public int MaxRowIndex { get; set; }
            public int i { get; set; }
            public int j { get; set; }
            public int group { get; set; }
        }

        # region 经测
        /// <summary>
        ///保存文件， 根据datatable生成EXCEL并合并相同列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path">路径</param>
        /// <param name="mergeCol">需要合并的列，不填则不合并 ，注意：从0开始计算</param>
        public static string MergeCellSaveExcel(DataTable dt, string path, params int[] mergeCol)
        {
            List<MergePar> mePars = new List<MergePar>();
            int group = 0;
            if (dt != null)
            {
                try
                {
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];
                    //为单元格添加样式      
                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    //设置居中  
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    //设置背景颜色  
                    style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.IsBold = true;
                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;
                    cellSheet.Cells.SetRowHeight(0, 30);
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        cellSheet.Cells.SetRowHeight(i, 25);
                    }
                    //列名的处理  
                    for (int i = 0; i < colCount; i++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;  
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";  
                        //cellSheet.Cells[rowIndex, colIndex].Style = style;  
                        colIndex++;
                    }
                    rowIndex++;
                    for (int i = 0; i < rowCount; i++)
                    {
                        colIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            string currentVal = dt.Rows[i][j].ToString();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);
                            //合并单元格
                            string currentKey = currentVal + colIndex + "" + j + "";

                            if (mePars.Any(c => c.Key == currentKey && c.group == group)) //更新合并参数
                            {
                                var data = mePars.Single(c => c.Key == currentKey && c.group == group);
                                data.MaxColIndex = colIndex;
                                data.MaxRowIndex = rowIndex;
                                data.i = i;
                                data.j = j;
                                data.group = group;
                            }
                            else
                            {
                                group++;
                                //插入新合并参数
                                MergePar m = new MergePar()
                                {
                                    Key = currentKey,
                                    MaxColIndex = colIndex,
                                    MaxRowIndex = rowIndex,
                                    MinColIndex = colIndex,
                                    MinRowIndex = rowIndex,
                                    j = j,
                                    i = i,
                                    group = group
                                };
                                mePars.Add(m);

                            }

                            //var range = cellSheet.Cells.CreateRange(i, j, 2, 1);
                            //Style s = new Style();
                            //s.VerticalAlignment = TextAlignmentType.Center;
                            //prevCell.SetStyle(s);
                            //range.Merge();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);



                            colIndex++;
                        }
                        rowIndex++;
                    }
                    cellSheet.AutoFitColumns();
                    path = Path.GetFullPath(path);
                    mePars = mePars.Where(c => c.MinRowIndex != c.MaxRowIndex).ToList();

                    //var range = cellSheet.Cells.CreateRange(2, 0,3, 1);
                    //Style s = new Style();
                    //s.VerticalAlignment = TextAlignmentType.Center;
                    //range.Merge();

                    foreach (var r in mePars)
                    {
                        if (mergeCol != null && mergeCol.Length > 0)
                        {
                            if (mergeCol.Contains(r.j))//只合并指定列
                            {
                                var range = cellSheet.Cells.CreateRange(r.MinRowIndex, r.MinColIndex, r.MaxRowIndex - (r.MinRowIndex - 1), 1);
                                Style s = new Style();
                                s.VerticalAlignment = TextAlignmentType.Center;
                                range.Merge();
                            }
                        }
                    }
                    workbook.Save(path);
                    return path;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "";
        }

        public static Style _thStyle
        {
            get
            {
                Style s = new Style();
                s.Font.IsBold = true;
                s.Font.Name = "宋体";
                s.Font.Color = Color.Black;
                s.HorizontalAlignment = TextAlignmentType.Center;  //标题居中对齐
                s.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                s.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                s.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                s.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                return s;
            }

        }

        public static Style _tdStyle
        {
            get
            {

                Style s = new Style();
                s.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                s.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                s.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                s.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                return s;
            }
        }

        /// <summary>
        ///导出， 根据datatable生成EXCEL并合并相同列 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path">路径</param>
        /// <param name="mergeCol">需要合并的列，不填则不合并 ，注意：从0开始计算</param>
        public static void MergeCellExport(DataTable dt, string name, params int[] mergeCol)
        {
            List<MergePar> mePars = new List<MergePar>();
            Dictionary<int, int> group = new Dictionary<int, int>();
            for (int i = 0; i < dt.Columns.Count; i++)//列分组编号
            {
                group.Add(i, 0);
            }
            if (dt != null)
            {
                try
                {
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];
                    //为单元格添加样式      
                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    //设置居中  
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    //设置背景颜色  
                    style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.IsBold = true;

                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;
                    cellSheet.Cells.SetRowHeight(0, 30);
                    for (int i = 1; i <= dt.Rows.Count+1; i++)
                    {
                        cellSheet.Cells.SetRowHeight(i, 25);
                    }



                

                    cellSheet.Cells[rowIndex,0].PutValue(name.Split('.')[0]);
                    cellSheet.Cells[rowIndex,5].PutValue(DateTime.Now.ToString("yyyy-MM-dd"));;
                    var cr= cellSheet.Cells.CreateRange(0,0,1,3);
                    cr.Merge();

                    rowIndex++;
                    //列名的处理  
                    for (int i = 0; i < colCount; i++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                        cellSheet.Cells[rowIndex, colIndex].SetStyle(_thStyle);
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;  
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";  
                        //cellSheet.Cells[rowIndex, colIndex].Style = style;  
                        colIndex++;
                    }


                    rowIndex++;
                    for (int i = 0; i < rowCount; i++)
                    {
                        colIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            string currentVal = dt.Rows[i][j].ToString();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);
                            cellSheet.Cells[rowIndex, colIndex].SetStyle(_tdStyle);
                            //合并单元格
                            string currentKey = currentVal + colIndex + "" + j + "";

                            if (mePars.Any(c => c.Key == currentKey && c.group == group[j])) //更新合并参数
                            {
                                var data = mePars.Single(c => c.Key == currentKey && c.group == group[j]);
                                data.MaxColIndex = colIndex;
                                data.MaxRowIndex = rowIndex;
                                data.i = i;
                                data.j = j;
                                data.group = group[j];
                            }
                            else
                            {
                                group[j] = group[j] + 1;
                                //插入新合并参数
                                MergePar m = new MergePar()
                                {
                                    Key = currentKey,
                                    MaxColIndex = colIndex,
                                    MaxRowIndex = rowIndex,
                                    MinColIndex = colIndex,
                                    MinRowIndex = rowIndex,
                                    j = j,
                                    i = i,
                                    group = group[j]
                                };
                                mePars.Add(m);

                            }

                            //var range = cellSheet.Cells.CreateRange(i, j, 2, 1);
                            //Style s = new Style();
                            //s.VerticalAlignment = TextAlignmentType.Center;
                            //prevCell.SetStyle(s);
                            //range.Merge();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);



                            colIndex++;
                        }
                        rowIndex++;
                    }
                    cellSheet.AutoFitColumns();
                    mePars = mePars.Where(c => c.MinRowIndex != c.MaxRowIndex).ToList();

                    //var range = cellSheet.Cells.CreateRange(2, 0,3, 1);
                    //Style s = new Style();
                    //s.VerticalAlignment = TextAlignmentType.Center;
                    //range.Merge();

                    foreach (var r in mePars)
                    {
                        if (mergeCol != null && mergeCol.Length > 0)
                        {
                            if (mergeCol.Contains(r.j))//只合并指定列
                            {
                                var range = cellSheet.Cells.CreateRange(r.MinRowIndex, r.MinColIndex, r.MaxRowIndex - (r.MinRowIndex - 1), 1);
                                Style s = new Style();
                                s.VerticalAlignment = TextAlignmentType.Center;
                                range.Merge();
                            }
                        }
                    }
                    var response = HttpContext.Current.Response;
                    response.Clear();
                    response.Buffer = true;
                    response.Charset = "utf-8";
                    response.AppendHeader("Content-Disposition", "attachment;filename=" + name);
                    response.ContentEncoding = System.Text.Encoding.UTF8;
                    response.ContentType = "application/ms-excel";
                    response.BinaryWrite(workbook.SaveToStream().ToArray());
                    response.End();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///导出， 根据datatable生成EXCEL并合并相同列 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path">路径</param>
        /// <param name="mergeCol">需要合并的列，不填则不合并 ，注意：从0开始计算</param>
        public static void MergeCellExportToPdf(DataTable dt, string name, params int[] mergeCol)
        {
            List<MergePar> mePars = new List<MergePar>();
            Dictionary<int, int> group = new Dictionary<int, int>();
            for (int i = 0; i < dt.Columns.Count; i++)//列分组编号
            {
                group.Add(i, 0);
            }
            if (dt != null)
            {
                try
                {
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];
                    //为单元格添加样式      
                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    //设置居中  
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    //设置背景颜色  
                    style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.IsBold = true;

                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;
                    cellSheet.Cells.SetRowHeight(0, 30);
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        cellSheet.Cells.SetRowHeight(i, 25);
                    }
                    //列名的处理  
                    for (int i = 0; i < colCount; i++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                        cellSheet.Cells[rowIndex, colIndex].SetStyle(_thStyle);
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;  
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";  
                        //cellSheet.Cells[rowIndex, colIndex].Style = style;  
                        colIndex++;
                    }
                    rowIndex++;
                    for (int i = 0; i < rowCount; i++)
                    {
                        colIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            string currentVal = dt.Rows[i][j].ToString();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);
                            cellSheet.Cells[rowIndex, colIndex].SetStyle(_tdStyle);
                            //合并单元格
                            string currentKey = currentVal + colIndex + "" + j + "";

                            if (mePars.Any(c => c.Key == currentKey && c.group == group[j])) //更新合并参数
                            {
                                var data = mePars.Single(c => c.Key == currentKey && c.group == group[j]);
                                data.MaxColIndex = colIndex;
                                data.MaxRowIndex = rowIndex;
                                data.i = i;
                                data.j = j;
                                data.group = group[j];
                            }
                            else
                            {
                                group[j] = group[j] + 1;
                                //插入新合并参数
                                MergePar m = new MergePar()
                                {
                                    Key = currentKey,
                                    MaxColIndex = colIndex,
                                    MaxRowIndex = rowIndex,
                                    MinColIndex = colIndex,
                                    MinRowIndex = rowIndex,
                                    j = j,
                                    i = i,
                                    group = group[j]
                                };
                                mePars.Add(m);

                            }

                            //var range = cellSheet.Cells.CreateRange(i, j, 2, 1);
                            //Style s = new Style();
                            //s.VerticalAlignment = TextAlignmentType.Center;
                            //prevCell.SetStyle(s);
                            //range.Merge();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);



                            colIndex++;
                        }
                        rowIndex++;
                    }
                    cellSheet.AutoFitColumns();
                    mePars = mePars.Where(c => c.MinRowIndex != c.MaxRowIndex).ToList();

                    //var range = cellSheet.Cells.CreateRange(2, 0,3, 1);
                    //Style s = new Style();
                    //s.VerticalAlignment = TextAlignmentType.Center;
                    //range.Merge();

                    foreach (var r in mePars)
                    {
                        if (mergeCol != null && mergeCol.Length > 0)
                        {
                            if (mergeCol.Contains(r.j))//只合并指定列
                            {
                                var range = cellSheet.Cells.CreateRange(r.MinRowIndex, r.MinColIndex, r.MaxRowIndex - (r.MinRowIndex - 1), 1);
                                Style s = new Style();
                                s.VerticalAlignment = TextAlignmentType.Center;
                                range.Merge();
                            }
                        }
                    }
                    var response = HttpContext.Current.Response;
                    string fileName = FileHelper.GetMapPath("~/Content/File/Temp/" + DateTime.Now.ToString("yyyyMMddHHmmss") + name);
                    workbook.Save(fileName, SaveFormat.Pdf);
                    FileHelper.ShowPDF(fileName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///保存， 根据datatable生成EXCEL并合并相同列 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path">路径</param>
        /// <param name="mergeCol">需要合并的列，不填则不合并 ，注意：从0开始计算</param>
        public static string MergeCellSavePdf(DataTable dt, string name, params int[] mergeCol)
        {
            List<MergePar> mePars = new List<MergePar>();
            Dictionary<int, int> group = new Dictionary<int, int>();
            for (int i = 0; i < dt.Columns.Count; i++)//列分组编号
            {
                group.Add(i, 0);
            }
            if (dt != null)
            {
                try
                {
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];
                    //为单元格添加样式      
                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    //设置居中  
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    //设置背景颜色  
                    style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.IsBold = true;

                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;
                    cellSheet.Cells.SetRowHeight(0, 30);
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        cellSheet.Cells.SetRowHeight(i, 25);
                    }
                    //列名的处理  
                    for (int i = 0; i < colCount; i++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                        cellSheet.Cells[rowIndex, colIndex].SetStyle(_thStyle);
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;  
                        //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";  
                        //cellSheet.Cells[rowIndex, colIndex].Style = style;  
                        colIndex++;
                    }
                    rowIndex++;
                    for (int i = 0; i < rowCount; i++)
                    {
                        colIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            string currentVal = dt.Rows[i][j].ToString();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);
                            cellSheet.Cells[rowIndex, colIndex].SetStyle(_tdStyle);
                            //合并单元格
                            string currentKey = currentVal + colIndex + "" + j + "";

                            if (mePars.Any(c => c.Key == currentKey && c.group == group[j])) //更新合并参数
                            {
                                var data = mePars.Single(c => c.Key == currentKey && c.group == group[j]);
                                data.MaxColIndex = colIndex;
                                data.MaxRowIndex = rowIndex;
                                data.i = i;
                                data.j = j;
                                data.group = group[j];
                            }
                            else
                            {
                                group[j] = group[j] + 1;
                                //插入新合并参数
                                MergePar m = new MergePar()
                                {
                                    Key = currentKey,
                                    MaxColIndex = colIndex,
                                    MaxRowIndex = rowIndex,
                                    MinColIndex = colIndex,
                                    MinRowIndex = rowIndex,
                                    j = j,
                                    i = i,
                                    group = group[j]
                                };
                                mePars.Add(m);

                            }

                            //var range = cellSheet.Cells.CreateRange(i, j, 2, 1);
                            //Style s = new Style();
                            //s.VerticalAlignment = TextAlignmentType.Center;
                            //prevCell.SetStyle(s);
                            //range.Merge();
                            cellSheet.Cells[rowIndex, colIndex].PutValue(currentVal);



                            colIndex++;
                        }
                        rowIndex++;
                    }
                    cellSheet.AutoFitColumns();
                    mePars = mePars.Where(c => c.MinRowIndex != c.MaxRowIndex).ToList();

                    //var range = cellSheet.Cells.CreateRange(2, 0,3, 1);
                    //Style s = new Style();
                    //s.VerticalAlignment = TextAlignmentType.Center;
                    //range.Merge();

                    foreach (var r in mePars)
                    {
                        if (mergeCol != null && mergeCol.Length > 0)
                        {
                            if (mergeCol.Contains(r.j))//只合并指定列
                            {
                                var range = cellSheet.Cells.CreateRange(r.MinRowIndex, r.MinColIndex, r.MaxRowIndex - (r.MinRowIndex - 1), 1);
                                Style s = new Style();
                                s.VerticalAlignment = TextAlignmentType.Center;
                                range.Merge();
                            }
                        }
                    }
                    var response = HttpContext.Current.Response;
                    string fileName = FileHelper.GetMapPath("~/Content/File/Temp/" + DateTime.Now.ToString("yyyyMMddHHmmss") + name);
                    workbook.Save(fileName, SaveFormat.Pdf);
                    return fileName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        /// <summary>
        /// Aspose.Cells 
        /// 导入到DataTable
        /// </summary>
        /// <param name="fileName">该文件所在路径</param>
        /// <returns>DataSet </returns>
        private DataSet GetDataFromExcel(string fileName)
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(fileType))
            {
                return null;
            }
            //if (fileType == ".xls")
            //{
            //    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName +";Extended Properties='Excel 8.0;IMEX=1'";
            // //   connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;IMEX =1\"";
            //}
            //else
            //{
            //    connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + fileName + ";" + "Extended Properties=\"Excel 12.0;HDR=YES;IMEX =1\"";
            //}
            //string sql_F = "Select   [Customer Item Code]   FROM [{0}]";
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            workbook.Open(fileName);
            Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
            Cells cells = worksheet.Cells;
            //ExportDataTableAsString ( Int32 firstRow, Int32 firstColumn, Int32 totalRows, Int32 totalColumns, Boolean exportColumnName )
            var datatable = worksheet.Cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            var reval = new DataSet();
            reval.Tables.Add(datatable);
            return reval;
        }
        # endregion

        #region 网上的
        /// <summary>
        /// aspose.cell导出 方法一
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="response"></param>
        public static void Export<T>(IEnumerable<T> data, HttpResponse response)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            PropertyInfo[] ps = typeof(T).GetProperties();
            var colIndex = "A";

            foreach (var p in ps)
            {

                sheet.Cells[colIndex + 1].PutValue(p.Name);
                int i = 2;
                foreach (var d in data)
                {
                    sheet.Cells[colIndex + i].PutValue(p.GetValue(d, null));
                    i++;
                }

                colIndex = ((char)(colIndex[0] + 1)).ToString();
            }

            response.Clear();
            response.Buffer = true;
            response.Charset = "utf-8";
            response.AppendHeader("Content-Disposition", "attachment;filename=xxx.xls");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            response.BinaryWrite(workbook.SaveToStream().ToArray());
            response.End();
        }


        /// <summary>
        /// aspose.cell导入
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static System.Data.DataTable ReadExcel(String strFileName)
        {
            Workbook book = new Workbook();
            book.Open(strFileName);
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;

            return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
        }

        #endregion


        public static Style GetStyle(Workbook workbook, int i)
        {

            #region 样式
            //为标题设置样式     
            if (i == 0||i==1)
            {
                Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
                styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                styleTitle.Font.Name = "宋体";//文字字体 
                styleTitle.Font.Size = 18;//文字大小 
                styleTitle.Font.IsBold = true;//粗体 
                return styleTitle;
            }

            if (i == 2)
            {
                //样式2 
                Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
                style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                style2.Font.Name = "宋体";//文字字体 
                style2.Font.Size = 14;//文字大小 
                style2.Font.IsBold = true;//粗体 
                style2.IsTextWrapped = true;//单元格内容自动换行 
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                return style2;
            }

            if (i == 3)
            {
                //样式3 
                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                style3.Font.Name = "宋体";//文字字体 
                style3.Font.Size = 12;//文字大小 
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                return style3;
            }
            return new Style();
            #endregion
        
        }
    }
}