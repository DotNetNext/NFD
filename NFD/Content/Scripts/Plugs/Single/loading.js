/** 
* @部分参数说明 
*/
(function ($) {
    $.fn.extend({
        //主函数  
        toggleLoading: function (options) {
            // 找到遮罩层  
            var crust = this.children(".x-loading-wanghe");
            // 当前操作的元素  
            var thisjQuery = this;
            // 实现toogle(切换遮罩层出现与消失)效果的判断方法  
            if (crust.length > 0) {
                if (crust.is(":visible")) {
                    crust.fadeOut(500);
                } else {
                    crust.fadeIn(500);
                }
                return this;
            }
            // 扩展参数  
            var op = $.extend({
                z: 99999999,
                msg: '数据加载中...',
                iconUrl: '/Content/Imgs/loading.gif',
                width: 18,
                height: 18,
                borderColor: '#6bc4f5',
                opacity: 0.5,
                agentW: thisjQuery.outerWidth(),
                agentH: thisjQuery.outerHeight()
            }, options);

            if (thisjQuery.css("position") == "static")
                thisjQuery.css("position", "relative");
            //var w = thisjQuery.outerWidth(),h = thisjQuery.outerHeight();  

            var w = op.agentW, h = op.agentH;
            crust = $("<div></div>").css({//外壳  
                'position': 'absolute',
                'z-index': op.z,
                'display': 'none',
                'width': w + 'px',
                'height': h + 'px',
                'text-align': 'center',
                'top': '0px',
                'left': '0px',
                'font-family': 'arial',
                'font-size': '12px',
                'font-weight': '500'
            }).attr("class", "x-loading-wanghe");

            var mask = $("<div></div>").css({//蒙版  
                'position': 'absolute',
                'z-index': op.z + 1,
                'width': '100%',
                'height': '100%',
                'background-color': '#333333',
                'top': '0px',
                'left': '0px',
                'opacity': op.opacity
            });
            //71abc6,89d3f8,6bc4f5  
            var msgCrust = $("<span></span>").css({//消息外壳  
                'position': 'relative',
                'top': (h - 30) / 2 + 'px',
                'z-index': op.z + 2,
                'height': '24px',
                'display': 'inline-block',
                'background-color': '#cadbe6',
                'padding': '2px',
                'color': '#000000',
                'border': '1px solid ' + op.borderColor,
                'text-align': 'left',
                'opacity': 0.9
            });
            var msg = $("<span>" + op.msg + "</span>").css({//消息主体  
                'position': 'relative',
                'margin': '0px',
                'z-index': op.z + 3,
                'line-height': '22px',
                'height': '22px',
                'display': 'inline-block',
                'background-color': '#efefef',
                'padding-left': '25px',
                'padding-right': '5px',
                'border': '1px solid ' + op.borderColor,
                'text-align': 'left',
                'text-indent': '0'
            });
            var msgIcon = $("<img src=" + op.iconUrl + " />").css({//图标  
                'position': 'absolute',
                'top': '3px',
                'left': '3px',
                'z-index': op.z + 4,
                'width': '18px',
                'height': '18px'
            });
            // 拼装遮罩层  
            msg.prepend(msgIcon);
            msgCrust.prepend(msg);
            crust.prepend(mask);
            crust.prepend(msgCrust);
            thisjQuery.prepend(crust);
            // alert(thisjQuery.html());  
            crust.fadeIn(500);
            //模态设置  
            return this;
        }
    });
})(jQuery);