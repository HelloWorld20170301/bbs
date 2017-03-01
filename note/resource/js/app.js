var tmp_ishow = false;
var menu_flag = 0;
$(function () {
    //初始化页面
    $(".menu").css("left", 0).css("height", $(document).height()).hide();
    $(".child_menu").hide();
    $(".content").css("width", window.innerWidth).css("height", window.innerHeight * 0.95);
    $(".footer").css("width", window.innerWidth).css("height", window.innerHeight * 0.05);

    //初始化菜单函数
    var InitMenu = function (data) {
        var menu = $("#menu_tree");
        var html = "";
        var config_buttons = '<ul>' +
                                    '<li class="menu_buttons_add" title="增加"><i class=" fa fa-plus"></i></li>' +
                                    '<li class="menu_buttons_remove" title="删除"><i class="fa fa-remove"></i></li>' +
                                    '<li class="menu_buttons_rename" title="重命名"><i class="fa fa-tag"></i></li>' +
                                '</ul>';
        for (var i = 0; i < data.length; i++) {
            var parent = data[i];
            html = '<div> ' +
                           '<div exp="0" class="menu_tip">' +
                               '<table>' +
                                   '<tr node_id="' + parent.MenuCode + '" node_type="Parent">' +
                                       '<td class="menu_des">' + parent.MenuName + '</td>' +
                                       '<td class="menu_icon"><i class="fa fa-chevron-right fa-lg"></i></td>' +
                                       '<td class="menu_buttons">' +
                                        config_buttons +
                                        '</td>' +
                                   '</tr>' +
                                   '</table>' +
                           '</div>' +
                           '<div class="child_menu">' +
                                '<ul>';
            for (var j = 0; j < parent.children.length; j++) {
                var child = parent.children[j];
                html += '<li class="child_menu_items">' +
                            '<table>' +
                                '<tr node_id="' + child.MenuCode + '" node_type="Child">' +
                                    '<td class="menu_des">' + child.MenuName + '</td>' +
                                    '<td class="child_menu_buttons">' +
                                        config_buttons +
                                        '</td>' +
                                    '</tr>' +
                            '</table>' +
                        '</li>';
            }
            html += '</ul></div>';
            menu.append(html);
        }
    }
    //初始化菜单项
    $.ajax({
        type: "post",
        url: "/Handler/NoteHandler.ashx?option=InitMenu",
        contentType: "application/json;charset=utf-8;",
        dataType: "json",
        async: false,
        success: function (data) {
            InitMenu(data);
            $(".menu_buttons_add").click(function (e) {
                zeroModal.show({
                    unique: 'add_modal',
                    title: '添加菜单项',
                    content: '名称<input type="text"  />',
                    ok: true,
                    width: '250px',
                    height: '120px',
                    okFn: function (opt) {
                        var tr = $($($(e.currentTarget).parent("ul")[0]).parent("td")).parent("tr");
                        var node_code = $(tr).attr("node_id");
                        var node_type = $(tr).attr("node_type");
                        var parentcode = "0";
                        if (node_type == "Child") {
                            parentcode = $(tr.parentNode.parentNode.parentNode.parentNode.parentNode.childNodes[0].childNodes[0].childNodes[0]).attr("node_id");
                        }
                        $.ajax({
                            type: "post",
                            url: "/Handler/NoteHandler.ashx?option=AddNode&parentcode=" + parentcode + "&type=" + node_type,
                            contentType: "application/json;charset=utf-8;",
                            dataType: "json",
                            success: function (data) {

                            }
                        });
                        zeroModal.close("add_modal");
                        tmp_ishow = false;
                        menu_flag = 1;
                        return false;
                    }
                });
                tmp_ishow = true;
            });
            $(".menu_buttons_remove").click(function (e) {
                zeroModal.confirm({
                    content: '确定删除吗？',
                    width: '350px',
                    height: '220px',
                    okFn: function () {
                        tmp_ishow = false;
                        menu_flag = 1;
                    },
                    cancelFn: function () {
                        tmp_ishow = false;
                        menu_flag = 1;
                    }
                });
                tmp_ishow = true;
            });
            $(".menu_buttons_rename").click(function (e) {
                zeroModal.show({
                    unique: 'add_modal',
                    title: '添加菜单项',
                    content: '名称<input type="text"  />',
                    ok: true,
                    width: '250px',
                    height: '120px',
                    okFn: function (opt) {
                        zeroModal.close("add_modal");
                        tmp_ishow = false;
                        menu_flag = 1;
                        return false;
                    }
                });
                tmp_ishow = true;
            });
        }
    });

    //显示或隐藏菜单配置按钮
    $("#config").click(function (e) {
        if ($(this).attr("exp") == "0") {
            $(".menu").animate({ "width": "260px" }, "fast");
            $("#menu_tree .menu_buttons ul").show("fast");
            $("#menu_tree .child_menu_buttons ul").show("fast");
            $(this).attr("exp", "1");
            return;
        }
        $(".menu").animate({ "width": "200px" }, "fast");
        $("#menu_tree .menu_buttons ul").hide("fast");
        $("#menu_tree .child_menu_buttons ul").hide("fast");
        $(this).attr("exp", "0");
    });

    //显示菜单
    $("#menu").click(function () {
        //$(".menu").css("height", $(document).height());
        $(".menu").show("fast");
    });
    //隐藏菜单
    $(document).click(function (e) {
        if (tmp_ishow) {
            return;
        } if (menu_flag == 1) {
            menu_flag = 0;
            return;
        }
        if (menu_flag == 0 && e.pageX > 260) {
            //$(".menu").animate({ "left": -500 }, 'normal');
            $(".menu").hide('fast');
        }
    });
    //菜单展开
    $(".menu_tip").click(function (e) {
        if (e.pageX > 180) {
            return;
        }
        var next = $(this).next();
        var menulength = next.find(".child_menu_items").length;
        var icon = $($(this).find("i")[0]);
        var parent = $(this).parent();
        if (this.attributes[0].value == "0" && next && menulength > 0) {
            icon.css("-webkit-transform", "rotate(90deg)").css("transition", "transform 0.3s linear 0s");
            parent.animate({ "height": menulength * 45 + 50 }, 'normal');
            next.show("fast");
            var next_menu = $($("#menu_tree").children().get(0));
            var child_menu = next_menu.find(".child_menu");
            while (next_menu.length > 0) {
                if (child_menu.find(".child_menu_items").length == 0 || !child_menu) {
                    next_menu = next_menu.next();
                    child_menu = next_menu.find(".child_menu");
                    continue;
                }
                var exp_menu = $(next_menu.children("div").get(0));
                if (exp_menu.attr("exp") == "1") {
                    next_menu.animate({ "height": 50 }, 'normal');
                    next_menu.find(".child_menu").hide("fast");
                    $(next_menu.find("i")[0]).css("-webkit-transform", "rotate(0deg)").css("transition", "transform 0.3s linear 0s");
                    exp_menu.attr("exp", "0");
                }
                next_menu = next_menu.next();
                child_menu = next_menu.find(".child_menu");
            }
            this.attributes[0].value = "1";
            return;
        }
        this.attributes[0].value = "0";
        icon.css("-webkit-transform", "rotate(0deg)").css("transition", "transform 0.3s linear 0s");
        parent.animate({ "height": 50 }, 'normal');
        next.hide("fast");
    });
})