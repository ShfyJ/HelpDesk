//window.onload(getUrl);
let hasNewMessage = true;
let countOfReq = 0;
(function ($) {

    // ****** Add ikr.notification.css ******
    $.fn.ikrNotificationSetup = function (options) {
        /*
          Declaration : $("#noti_Container").ikrNotificationSetup({
                    List: objCollectionList
          });
       */
        var defaultSettings = $.extend({
            BeforeSeenColor: "#2E467C",
            AfterSeenColor: "#ccc"
        }, options);
        $(".ikrNoti_Button").css({
            "background": defaultSettings.BeforeSeenColor
        });
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId).append("<div class='ikrNoti_Counter'></div>" +
                "<div class='ikrNoti_Button'></div>" +
                "<div class='ikrNotifications'>" +
                "<h3>&#1061&#1072&#1073&#1072&#1088&#1083&#1072&#1088 (<span class='notiCounterOnHead'>0</span>)</h3>" +
                "<div class='ikrNotificationItems'>" +
                "</div>" +
                "<div class='ikrSeeAll'><a href=''>&#1041&#1072&#1088&#1095&#1072&#1089&#1080</a></div>" +
                "</div>");

            $('#' + parentId + ' .ikrNoti_Counter')
                .css({ opacity: 0 })
                .text(0)
                .css({ top: '-10px' })
                .animate({ top: '-2px', opacity: 1 }, 500);

            $('#' + parentId + ' .ikrNoti_Button').click(function () {
                $('#' + parentId + ' .ikrNotifications').fadeToggle('fast', 'linear', function () {
                    if ($('#' + parentId + ' .ikrNotifications').is(':hidden')) {
                        $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.AfterSeenColor);
                    }
                    else $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.BeforeSeenColor);
                });
                $('#' + parentId + ' .ikrNoti_Counter').fadeOut('slow');
                return false;
            });
            $(document).click(function () {
                $('#' + parentId + ' .ikrNotifications').hide();
                if ($('#' + parentId + ' .ikrNoti_Counter').is(':hidden')) {
                    $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.AfterSeenColor);
                }
            });
            $('#' + parentId + ' .ikrNotifications').click(function () {
                return false;
            });

            $("#" + parentId).css({
                position: "relative"
            });
        }
    };
    $.fn.ikrNotificationCount = function (options) {
        /*
          Declaration : $("#myComboId").ikrNotificationCount({
                    NotificationList: [],
                    NotiFromPropName: "",
                    ListTitlePropName: "",
                    ListBodyPropName: "",
                    ControllerName: "Notifications",
                    ActionName: "AllNotifications"
          });
       */

        var defaultSettings = $.extend({
            NotificationList: [],
            NotiFromPropName: "",
            ListTitlePropName: "",
            ListBodyPropName: "",
            AreaName: "Manager",
            ControllerName: "RequestManager",
            ActionName: "AllNotifications"
        }, options);
        
        var Area = "HeadManager";
        var Controller= "Requests";
       
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            

            $("#" + parentId + " .ikrNotifications .ikrSeeAll").click(function () {
                window.open('/');
            });
            
            var totalUnReadNoti = defaultSettings.NotificationList.filter(x => x.rStatus != "green").length;
            $('#' + parentId + ' .ikrNoti_Counter').text(totalUnReadNoti);
            $('#' + parentId + ' .notiCounterOnHead').text(totalUnReadNoti);
            
            console.log(countOfReq);
            console.log(defaultSettings.NotificationList.length);

            if (defaultSettings.NotificationList.length > 0) {

                if (countOfReq != defaultSettings.NotificationList.length) {
                 
                    countOfReq = defaultSettings.NotificationList.length;
                    if (Notification.permission === "granted") {
                        hasNewMessage = true;
                        showNoti(JSON.stringify(defaultSettings.NotificationList));
                        console.log(JSON.stringify(defaultSettings.NotificationList));
                    }
                    else if (Notification.permission !== "denied") {
                        Notification.requestPermission().then(permission => {
                            if (permission === "granted") {
                                showNoti(JSON.stringify(defaultSettings.NotificationList));
                            }
                        })
                    }
                }
                $("#" + parentId + " .ikrNotificationItems").empty();
                $.map(defaultSettings.NotificationList, function (item) {
                    var status = "";
                    if (item[ikrLowerFirstLetter(defaultSettings.ListBodyPropName)] == "blue") {
                        status = "&#1061&#1086&#1076&#1080&#1084 &#1073&#1080&#1088&#1080&#1082&#1090&#1080&#1088&#1080&#1083&#1075&#1072&#1085"
                    }
                    else if (item[ikrLowerFirstLetter(defaultSettings.ListBodyPropName)] == "Taken") {
                        status = "&#1061&#1086&#1076&#1080&#1084 &#1179&#1072&#1073&#1091&#1083 &#1179&#1080&#1083&#1076&#1080"
                    }
                    else if (item[ikrLowerFirstLetter(defaultSettings.ListBodyPropName)] == "yellow") {
                        status = "&#1061&#1086&#1076&#1080&#1084 &#1073&#1080&#1088&#1080&#1082&#1090&#1080&#1088&#1080&#1083&#1076&#1080"
                    }
                    else if (item[ikrLowerFirstLetter(defaultSettings.ListBodyPropName)] == "red") {
                        status = "&#1061&#1086&#1076&#1080&#1084 &#1088&#1072&#1076 &#1101&#1090&#1076&#1080"
                    }
                    var className = item.isRead ? "" : " ikrSingleNotiDivUnReadColor";
                   var sNotiFromPropName = $.trim(defaultSettings.NotiFromPropName) == "" ? "" : item[ikrLowerFirstLetter(defaultSettings.NotiFromPropName)];
                    $("#" + parentId + " .ikrNotificationItems").append("<div class='ikrSingleNotiDiv" + className + "' notiId=" + item.requestId + ">" +
                            "<h4 class='ikrNotiFromPropName'>" + sNotiFromPropName + "</h4>" +
                        "<h5 class='ikrNotificationTitle'>" + item[ikrLowerFirstLetter(defaultSettings.ListTitlePropName)] + "</h5>" +
                        "<div class='ikrNotificationBody'>" + status + "</div>" +
                            "<div class='ikrNofiCreatedDate'>" + item.date + "</div>" +
                        "</div>");
                   // console.log(window.location.search.substr(1))
                    var a = window.location.pathname;
                    console.log(a.slice(0, 2));
                    $("#" + parentId + " .ikrNotificationItems .ikrSingleNotiDiv[notiId=" + item.requestId + "]").click(function () {
                        
                        if ($.trim(item.requestId) != "") {
                            if (a == "/Identity/Account/Manage") {
                                window.location.pathname = window.location.pathname;
                            }
                            //if (a.slice(0,26) == "/HeadManager/Requests/Edit" || a == "/HeadManager/Requests/dashboard" || a == "/HeadManager/Requests" || a == "/HeadManager/Workers" || a == "/HeadManager/Workers/contacts" || a == "/HeadManager/Workers/Reports") {
                           // if (a.slice(0,2) == "/M") {
                            //window.location.pathname = "Manager/RequestManager/Edit/" + item.requestId;
                            //}
                          

                            else if (a.slice(0, 4) == "/Man") {
                                window.location.pathname = "Manager/RequestManager/Edit/" + item.requestId;
                                console.log(window.location.pathname);
                            }
                            else if (a.slice(0,10) == "/Requester") {
                                window.location.pathname = "Requester/RequestSSender/Edit/" + item.requestId;
                            }
                            else if (a.slice(0,9) == "/Employee") {
                                window.location.pathname = "/Employee/Requests/Edit/" + item.requestId;
                            }
                            else {
                                window.location.pathname = "/HeadManager/Requests/Edit/" + item.requestId;
                            }
                            }
                        else
                            window.location.href = "/Manager/RequestManager";


                        });
                    
                    $("#ikrNotificationItems").empty();
                 
                    
                });
             
                
            }
            
            
        }
    };
}(jQuery));


function showNoti(title) {
    var obj = JSON.parse(title);
    
    if (hasNewMessage) {
       
        const notificaiton = new Notification("HelpDesk", {
            
            icon: '/favicon.ico',
            body: obj[0]["rName"],

        })
       
        hasNewMessage = false;
    }
}
function ikrLowerFirstLetter(value) {
    
    return value.charAt(0).toLowerCase() + value.slice(1);
}


var url = '';

function getRole(role) {
    role = "hdasd";
    var userRole;
    if (userRole = '@(User.IsInRole(SD.Role_Manager))'){
        role="Manager"
    }
    if (userRole = '@(User.IsInRole(SD.Role_HeadManager))'){
        role = "HeadManager"
    }

    return role;
}

function getUrl() {
    if (getRole() == 'Manager') {
        url = 'Manager/RequestManager';
    }

    if (getRole() == 'HeadManager') {
        url = 'HeadManager/Requests';
    }
    else
        url = 'Manager/RequestManager';
    
    return url;
}
