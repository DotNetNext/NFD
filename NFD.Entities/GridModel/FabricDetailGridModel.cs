using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trirand.Web.Mvc;

namespace NFD.Entities.GridModel
{
    public class FabricDetailGridModel
    {
        /// <summary>
        /// 获取面料详情
        /// </summary>
        public JQGrid GetFabricDetailGridModel
        {
            get
            {
                var reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridWindowHeight;
                reval.Height = height;
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,
                    TopOffset = 200,
                    LeftOffset = 200

                };
                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "fd_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowAddButton = true,
                    ShowDeleteButton = true
                };
                reval.DataUrl = Url.Action("GetFDGridData");
                reval.EditUrl = Url.Action("EditFDGridData");
                reval.Columns = new List<JQGridColumn>();
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fd_id",
                    PrimaryKey = true,
                    Editable = false,
                    HeaderText = "编号"

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "color_name",
                    Editable = true,
                    HeaderText = "色号",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator()
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_quantity",
                    Editable = true,
                    HeaderText = "订货数量(米)",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new NumberValidator()
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "subtotal",
                    Editable = true,
                    HeaderText = "小计",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_arrival",
                    Editable = true,
                    HeaderText = "面料到货",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_add_reduction",
                    Editable = true,
                    HeaderText = "面料增减",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "clothes_orders_num",
                    Editable = true,
                    HeaderText = "成衣订单数",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "consumption",
                    Editable = true,
                    HeaderText = "单耗",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "mf",
                    Editable = true,
                    HeaderText = "有效门幅",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_date",
                    Editable = true,
                    HeaderText = "交货时间",
                    EditFieldAttributes = new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                },

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "creator_name",
                    Editable = false,
                    HeaderText = "创建人"


                });
                return reval;
            }

        }
    }
}
