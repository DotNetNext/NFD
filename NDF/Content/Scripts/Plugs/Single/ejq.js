/*!
* ejq Library v0.1
* 说明：http://www.52mvc.com/showtopic-5606.aspx
* 时间: sunkaixuan 2014-8-22  
*/
(function (window, jQuery, undefined) {
    var ejq = window.ejq = jQuery;
    /*********************************元素操作*********************************/
    ejq.fn.extend({
        /*input*/
        //对input进行Disabled操作
        disabled: function (isDisabled) {
            if (isDisabled) {
                this.attr("disabled", "disabled");
            } else {
                this.removeAttr("disabled", "disabled");
            }
        },
        /*不限*/
        //将当前元素移动到指定元素的 上下左右
        nextTo: function (o) {
            /*
            ejq("div").ejqNextTo({
            position:"dwon",//将div移动到a的下方 参数:top down left right 默认dwon
            container:$("a"),
            appendLeft:0,//可以微调
            appendTop:0
            });
            */
            if (o.position == null) {
                o.position = "dwon";
            }
            if (o.appendTop == null) {
                o.appendTop = 0;
            }
            if (o.appendLeft == null) {
                o.appendLeft = 0;
            }
            var p = o.position; //移动容器位置
            var c = o.container; //移动到该容器上下左右
            var left, top;
            var coff = c.offset();
            var ch = c.height();
            var cw = c.width();
            var th = ejq(this);
            var thw = th.width();
            var thh = th.height();
            if (p == "dwon") {
                left = coff.left;
                top = coff.top + ch;
            } else if (p == "top") {
                left = coff.left;
                top = coff.top - thh;
            } else if (p == "left") {
                left = coff.left - thw;
                top = coff.top;
            }
            else if (p == "right") {
                left = coff.left + cw;
                top = coff.top;
            }
            this.css({ left: left + o.appendLeft, top: top + o.appendTop })
        },
        //判段坐标是否在当前元素内
        isInCurrentEle: function (x, y) {
            var th = this;
            var thOff = th.offset();
            if (thOff == null) return false;
            var left = thOff.left;
            var top = thOff.top;
            var thw = th.width();
            var thh = th.height();
            var isXin = x > (left - 30) && x < (left + thw + 30);
            var isYin = y > (top - 30) && y < (top + thh + 30);
            return isXin && isYin;
        },
        //元素窗口居中
        eleMiddle: function () {
            var th = this;
            var htmlObjH = th.height();
            var htmlObjW = th.width();
            var w = ejq(window).width();
            var scrollTop = ejq(document).scrollTop();
            var h = ejq(window).height() + scrollTop;
            var left = (w - htmlObjW) / 2;
            var top = (h - htmlObjH) / 2;
            th.css({ position: "absolute", "z-index": 1000000, left: left, top: top });
        }

    })

    /*********************************变量操作*********************************/


    ejq.extend({
        /*linq*/
        linq: {
            contains: function (thisVal, cobj) {
                if (ejq.valiData.isEmpty(thisVal)) {
                    return false;
                }
                return thisVal.toString().lastIndexOf(cobj.toString()) != -1;
            },
            /*where*/
            where: function (obj, action) {
                if (action == null) return;
                var reval = new Array();
                $(obj).each(function (i, v) {
                    if (action(v)) {
                        reval.push(v);
                    }
                })
                return reval;
            },
            /*any*/
            any: function (obj, action) {
                if (action == null) return;
                var reval = false;
                $(obj).each(function (i, v) {
                    if (action(v)) {
                        reval = true;
                        return false;
                    }
                })
                return reval;
            },
            /*select*/
            select: function (obj, action) {
                if (action == null) return;
                var reval = new Array();
                $(obj).each(function (i, v) {
                    reval.push(action(v));
                });
                return reval;
            },
            /*each*/
            each: function (obj, action) {
                if (action == null) return;
                ejq(obj).each(function (i, v) {
                    action(i, v);
                });
            },
            /*first*/
            first: function (obj, action) {
                if (action == null) return;
                var reval = new Array();
                $(obj).each(function (i, v) {
                    if (action(v)) {
                        reval.push(v);
                        return false;
                    }
                })
                return reval;
            }

        },

        /*操作*/
        action: {
            //移除最后一个字符
            trimEnd: function (str, c) {
                var reg = new RegExp(c + "([^" + c + "]*?)$");
                return str.replace(reg, function (w) { if (w.length > 1) { return w.substring(1); } else { return ""; } });
            },
            htmlEncode: function (str) {
                return str.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
            },
            htmlDecode: function (str) {
                return str.replace(/&amp;/g, '&').replace(/&quot;/g, '\"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
            },
            textEncode: function (str) {
                str = str.replace(/&amp;/gi, '&');
                str = str.replace(/</g, '&lt;');
                str = str.replace(/>/g, '&gt;');
                return str;
            },
            textDecode: function (str) {
                str = str.replace(/&amp;/gi, '&');
                str = str.replace(/&lt;/gi, '<');
                str = str.replace(/&gt;/gi, '>');
                return str;
            },
            //获取json的key和value
            jsonDictionary: function (json, key) {
                var reval = new Array();
                for (key in json) {
                    reval.push({ key: key, value: json[key] });
                }
                return reval;
            }

        },
        /*转换*/
        convert: {
            //还原json格式的时间
            jsonReductionData: function (cellval) {
                try {
                    if (cellval == "" || cellval == null) return "";
                    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
                    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                    return date.getFullYear() + "-" + month + "-" + currentDate;
                } catch (e) {
                    return "";
                }
            },
            jsonToStr: function (object) {
                var type = typeof object;
                if ('object' == type) {
                    if (Array == object.constructor) type = 'array';
                    else if (RegExp == object.constructor) type = 'regexp';
                    else type = 'object';
                }
                switch (type) {
                    case 'undefined':
                    case 'function':
                    case 'unknown':
                        return;
                        break;
                    case 'function':
                    case 'boolean':
                    case 'regexp':
                        return object.toString();
                        break;
                    case 'number':
                        return isFinite(object) ? object.toString() : 'null';
                        break;
                    case 'string':
                        return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g, function () {
                            var a = arguments[0];
                            return (a == '\n') ? '\\n' : (a == '\r') ? '\\r' : (a == '\t') ? '\\t' : ""
                        }) + '"';
                        break;
                    case 'object':
                        if (object === null) return 'null';
                        var results = [];
                        for (var property in object) {
                            var value = ejq.ejqConvert.jsonToStr(object[property]);
                            if (value !== undefined) results.push(ejq.ejqConvert.jsonToStr(property) + ':' + value);
                        }
                        return '{' + results.join(',') + '}';
                        break;
                    case 'array':
                        var results = [];
                        for (var i = 0; i < object.length; i++) {
                            var value = ejq.ejqConvert.jsonToStr(object[i]);
                            if (value !== undefined) results.push(value);
                        }
                        return '[' + results.join(',') + ']';
                        break;
                }
            },
            strToJson: function (str) {
                return ejq.parseJSON(str);
            }
        },
        /*数据验证*/
        valiData: {
            isEmpty: function (val) { return val == null || val == "" || val.toString() == ""; },
            isZero: function (val) { return val == null || val == "" || val == 0 || val == "0"; },
            //判断是否为数字
            isNumber: function (val) { return (/^\d+$/.test(val)); },
            //是否是邮箱
            isMail: function (val) { return (/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(val)); },
            //是否是手机
            isMobilePhone: function (val) { return (/\d{11}$/.test(val)); },
            //判断是否为负数和整数
            isNumberOrNegative: function (val) { return (/^\d+|\-\d+$/.test(val)); },
            //金额验证
            isMoney: function (val) { return (/^[1-9]d*.d*|0.d*[1-9]d*|\d+$/.test(val)); }

        },
        /*类型验证*/
        valiType: {
            isArray: function (obj) { return (typeof obj == 'object') && obj.constructor == Array; },
            isString: function (str) { return (typeof str == 'string') && str.constructor == String; },
            isDate: function (obj) { return (typeof obj == 'object') && obj.constructor == Date; },
            isFunction: function (obj) { return (typeof obj == 'function') && obj.constructor == Function; },
            isObject: function (obj) { return (typeof obj == 'object') && obj.constructor == Object; }
        }

    });

    /*********************************form操作*********************************/
    ejq.fn.extend({
        //获取元素属性以","隔开
        attrToStr: function (attr) {
            var reval = "";
            this.each(function () {
                reval += ejq(this).attr(attr) + ","
            })
            reval = ejq.ejqAction.trimEnd(reval, ",");
            return reval;
        },
        //将json对象自动填充到表单 
        //例如 $.formFill({data:{id:1},prefix:"user."}) 填充后  <input name='user.id' value='1' >
        formFill: function (option) {
            var prefix = option.prefix;
            var frmData = option.data;
            for (i in frmData) {
                var dataKey = i;
                var thisData = ejq("[name='" + prefix + i + "']");
                var text = "text";
                var hidden = "hidden";
                if (thisData != null) {
                    var thisDataType = thisData.attr("type");
                    var val = frmData[i];
                    var isdata = (val != null && val.toString().lastIndexOf("/Date(") != -1);
                    if (thisDataType == "radio") {
                        thisData.filter("[value=" + val + "]").attr("checked", "checked")
                        if (val == true || val == "0") val = "True";
                        else if (val == false || val != "0") val = "False";
                        thisData.filter("[value=" + val + "]").not("donbool").attr("checked", "checked")
                    } else if (thisDataType == "checkbox") {
                        if (thisData.size() == 1) {
                            if (val == "true" || val == 1 || val == "True" || val == "1") {
                                thisData.attr("checked", "checked");
                            } else {
                                thisData.removeAttr("checked");
                            }
                        } else {

                            thisData.removeAttr("checked");
                            var cbIndex = i;
                            if (val.lastIndexOf(",") == -1) {
                                ejq("[name='" + prefix + dataKey + "'][value='" + prefix + val + "']").attr("checked", "checked");
                            } else {
                                ejq(val.split(',')).each(function (i, v) {
                                    ejq("[name='" + prefix + dataKey + "'][value='" + prefix + v + "']").attr("checked", "checked"); ;
                                })
                            }
                        }

                    } else {
                        if (isdata) {
                            val = ejq.ejqConvert.jsonReductionData(val);
                        }
                        if (val == "null" || val == null)
                            val = "";
                        if (val == "" && thisData.attr("watertitle") == thisData.val()) {
                        } else {
                            thisData.val(val + "");
                            thisData.removeClass("watertitle")
                        }
                    }
                }

            }

        }


    });

    /*********************************ajax操作*********************************/
    ejq.extend({
        ajax: {
            error: function (msg, action) {
                if (action != null) {
                    action(msg);
                }
                try {
                    console.log(msg);
                } catch (e) {

                }
            }
        }
    });
    /*********************************浏览器操作*********************************/
    ejq.extend({
        /*requst对象*/
        request: {
            queryString: function () {
                var s1;
                var q = {}
                var s = document.location.search.substring(1);
                s = s.split("&");
                for (var i = 0, l = s.length; i < l; i++) {
                    s1 = s[i].split("=");
                    if (s1.length > 1) {
                        var t = s1[1].replace(/\+/g, " ")
                        try {
                            q[s1[0]] = decodeURIComponent(t)
                        } catch (e) {
                            q[s1[0]] = unescape(t)
                        }
                    }
                }
                return q;
            },
            url: function () {
                return window.location.href;
            },
            domain: function () {
                return window.location.host;
            },
            pageName: function () {
                var a = location.href;
                var b = a.split("/");
                var c = b.slice(b.length - 1, b.length).toString(String).split(".");
                return c.slice(0, 1);
            },
            pageFullName: function () {
                var strUrl = location.href;
                var arrUrl = strUrl.split("/");
                var strPage = arrUrl[arrUrl.length - 1];
                return strPage;
            }
        },
        response: {
            //页面跳转
            redirect: function (url, params) {
                if (params == null || params == "") {
                    window.location.href = url;
                } else {
                    if (ejq.ejqContains(url.toString(), "?")) {
                        var rurl = url + "&" + ejq.param(params);
                        window.location.href = rurl;
                    } else {
                        var rurl = url + "?" + ejq.param(params);
                        window.location.href = rurl;
                    }
                }
            }

        },
        /*浏览器判段*/
        broVali: {
            isIE6: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "6.0")
                    flag = true;
                return flag;
            },
            isIE7: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "7.0")
                    flag = true;
                return flag;
            },
            isIE8: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "8.0")
                    flag = true;
                return flag;
            },
            isIE9: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "9.0")
                    flag = true;
                return flag;
            },
            isIE10: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "10.0")
                    flag = true;
                return flag;
            },
            isMozilla: function () {
                var flag = false;
                if ($.browser.mozilla)
                    flag = true;
                return flag;
            },
            isOpera: function () {
                var flag = false;
                if ($.browser.opera)
                    flag = true;
                return flag;
            },
            isSafri: function () {
                var flag = false;
                if ($.browser.safari)
                    flag = true;
                return flag;
            },
            isMobile: function () {
                var userAgentInfo = navigator.userAgent;
                var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                var flag = false;
                for (var v = 0; v < Agents.length; v++) {
                    if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = true; break; }
                }

                return flag;
            },
            isIPhone: function () {
                var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                return ejq.ejqAny(Agents, function (v) {
                    return v == "iPhone";
                });
            },
            isAndroid: function () {
                var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                return ejq.ejqAny(Agents, function (v) {
                    return v == "Android";
                });
            }
        },
        //打印
        print: function (id/*需要打印的最外层元素ID*/) {
            var el = document.getElementById(id);
            var iframe = document.createElement('IFRAME');
            var doc = null;
            iframe.setAttribute('style', 'position:absolute;width:0px;height:0px;left:-500px;top:-500px;');
            document.body.appendChild(iframe);
            doc = iframe.contentWindow.document;
            doc.write('<div>' + el.innerHTML + '</div>');
            doc.close();
            iframe.contentWindow.focus();
            iframe.contentWindow.print();
            if (navigator.userAgent.indexOf("MSIE") > 0) {
                document.body.removeChild(iframe);
            }
        },
        //加入收藏夹
        addFavorite: function (surl, stitle) {
            try {
                window.external.addFavorite(surl, stitle);
            } catch (e) {
                try {
                    window.sidebar.addpanel(stitle, surl, "");
                } catch (e) {
                    alert("加入收藏失败,请使用ctrl+d进行添加");
                }
            }
        },
        //设为首页
        setHome: function (obj, vrl) {
            try {
                obj.style.behavior = 'url(#default#homepage)';
                obj.sethomepage(vrl);
            } catch (e) {
                if (window.netscape) {
                    try {
                        netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                    } catch (e) {
                        alert("此操作被浏览器拒绝!\n请在浏览器地址栏输入'about:config'并回车\n然后将[signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
                    }
                } else {
                    alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动设置为首页。");
                }
            }
        }
    });

    /*********************************常用插件*********************************/
    ejq.fn.extend({
        page: function (total, pageSize, pageIndex, action) {  //计算出分页
            window.ejqAjaxPage = action;
            var allPage = 0;
            var next = 0;
            var prev = 0;
            var pagestr = "";
            var startCount = 0;
            var endCount = 0;
            if (pageIndex < 1) { pageIndex = 1; }
            if (pageSize != 0) {
                allPage = parseInt(total / pageSize); //去掉小数
                allPage = ((total % pageSize) != 0 ? allPage + 1 : allPage);
                allPage = (allPage == 0 ? 1 : allPage);
            }
            next = pageIndex + 1;
            prev = pageIndex - 1;
            startCount = (pageIndex + 5) > allPage ? allPage - 9 : pageIndex - 4;
            endCount = pageIndex < 5 ? 10 : pageIndex + 5;
            if (startCount < 1) { startCount = 1; }
            if (allPage < endCount) { endCount = allPage; }
            pagestr += pageIndex > 1 ? "<a class=\"paginate_button\" data-index=\"1\" >首页</a><a class=\"paginate_button\" data-index=\"" + prev + "\"  >上一页</a>" : "";
            for (var i = startCount; i <= endCount; i++) {
                pagestr += pageIndex == i ? "<a id=\"current\">" + i + "</a>" : "<a class=\"paginate_button\" data-index=\"" + i + "\" >" + i + "</a>";
            }
            pagestr += pageIndex != allPage ? "<a class=\"paginate_button\"  data-index=\"" + next + "\" >下一页</a><a class=\"paginate_button\" data-index=\"" + allPage + "\">尾页</a>" : "";
            var selectHtml = "<select><option " + (pageSize == 5 ? "selected=\"selected\"" : "") + " >5</option><option " + (pageSize == 10 ? "selected=\"selected\"" : "") + ">10</option><option " + (pageSize == 20 ? "selected=\"selected\"" : "") + ">20</option><option " + (pageSize == 30 ? "selected=\"selected\"" : "") + ">30</option><option " + (pageSize == 50 ? "selected=\"selected\"" : "") + ">50</option><option " + (pageSize == 100 ? "selected=\"selected\"" : "") + ">100</option></select>";
            this.addClass("ejqAjaxPage")
            this.html("<span>共" + total + "条记录" + selectHtml + "</span>" + pagestr)
            ejq(".ejqAjaxPage a").live("click", function () {
                debugger
                if (action != null) {
                    var th = ejq(this);
                    var index = th.attr("data-index");
                    action(index);
                }
            });
        },
        loading: function () { },
        menu: function () { },
        mask: function () { },
        grid: function () { },
        water: function () { }
    });

    /*********************************初始化*********************************/
    ejq.init = function () {
        String.prototype.format = function (args) {
            var _dic = typeof args === "object" ? args : arguments;
            return this.replace(/\{([^{}]+)\}/g, function (str, key) {
                return _dic[key] || str;
            });
        }
        String.prototype.append = function (args) {
            return this + args;
        }
        String.prototype.appendFormat = function (appendValue, appendArgs) {
            return this + appendValue.format(appendArgs);
        }
    }
    ejq.init();

})(window, jQuery)
