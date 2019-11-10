var friend_return=0;
//var keep_count=0;

var userName="none";
var md5="";
var log_msg="";
var session_send=0;
var head="";
var sep=1;
var current_path="/root/happyli";//当前路径

[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]


[[[event_chat.js]]]



function event_msg(data){
    var obj=JSON.parse(data);
    var msg=obj.message;
    
    if (obj.k=="1"){
        return ;
    }
    switch(obj.type){
        case "chat_return":
            //log_msg="<b>chat_return:"+obj.oid+"</b><br>"+log_msg;
            //s_ui.Web_Content("web",css_head+log_msg);
            s_ui.status_label_show("status_label","chat_return:"+obj.oid);
            delete myMap["K"+obj.oid];
            break;
        case "login.ok":
            //s_ui.msg("login.ok friend list");
            friend_list("login.ok");
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            friend_return=1;
            break;
        case "file_sql":
            if (obj.message=="finished"){
                
                log_msg="<b>finished "+obj.from+";"+step+"</b><br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
            }
            break;
        case "msg":
            event_chat(data);
            break;
        case "ai_return":
            log_msg="<b>ai:"+msg+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            break;
        case "read_return":
            break;
        case "status":
            switch(obj.from){
                case "progress1":
                    var strSplit=msg.split(":");
                    s_ui.progress_show("progress1","100",strSplit[0]);
                    s_ui.progress_show("progress2","100","100");
                    break;
                case "progress2":
                    var strSplit=msg.split(":");
                    s_ui.progress_show('progress2', "100",strSplit[0]);
                    break;
            }
            break;
        default:
            log_msg=s_time.Time_Now()
                +"<span style='color:red;'>"+data+"</span><br>"
                +"<br><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            break;
    }
}


function New_URL(data){
    var strSplit=data.split("?");
    var file=strSplit[1];
    switch(strSplit[0]){
        case "http://file.edit_group/":
            var file2=file.substr(5);
            change_group(file2);
            break;
        case "http://file.edit/":
            var file2=file.substr(5);
            edit_file(file2);
            break;
    }
}



function friend_list(data){
    //s_ui.msg("friend_list"+data);
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");

    send_msg("friend_list","","","friend_list");
}

//发送消息
function send_msg_click(){
    msg_id+=1;
    
    
    var strMsg=s_ui.text_read("txt_send");
    
    var msg="{ \"id\":\"1\","
        +"\"command\":\"ai.answer\","
        +"\"topic\":\"\","
        +"\"content\":\""+strMsg+"\","
        +"\"web\":\"1\","
        +"\"qq\":\"0\","
        +"\"param\":\"\"}";

    var url="http://robot6.funnyai.com:8088/Get.FunnyAI?F=Get.FunnyAI&AI=1&Proxy=SYS";
    var data="msg="+s_string.urlencode(msg);
    var strReturn=s_net.http_post(url,data);
    
    log_msg=strReturn+"<br>"+log_msg;
    s_ui.Web_Content("web",css_head+log_msg);
    
    s_ui.text_set("txt_send","");
}


function send_msg_click2(){
    msg_id+=1;
    
    
    var strMsg=s_ui.text_read("txt_send");
    
    var msg="{ \"id\":\"1\","
        +"\"command\":\"ai.answer\","
        +"\"topic\":\"\","
        +"\"content\":\""+strMsg+"\","
        +"\"web\":\"1\","
        +"\"qq\":\"0\","
        +"\"param\":\"\"}";

    var url="http://127.0.0.1:8099/Get.FunnyAI?F=Get.FunnyAI&AI=1&Proxy=SYS";
    var data="msg="+s_string.urlencode(msg);
    var strReturn=s_net.http_post(url,data);
    
    log_msg=strReturn+"<br>"+log_msg;
    s_ui.Web_Content("web",css_head+log_msg);
    
    s_ui.text_set("txt_send","");
}


function send_msg(strType,friend,msg,return_cmd){
    msg_id+=1;
    
    var token=sys_get_token();
    var strLine="";
    var strSplit=msg.split(" ");
    var cmd=strSplit[0];
    switch(cmd){
        case "ls":
            current_path=strSplit[strSplit.length-1];
            if (current_path.endsWith("/")){
                current_path=current_path.substr(0,current_path.length-1);
            }
            sep="file_list";//分隔符用第1.1个方案
            head="<tr><th>权限</th><th>子文件数</th><th>用户</th><th>用户组</th><th>大小</th><th colspan=3>最近修改/查看时间</th><th>名称</th></tr>";
            break;
    }
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    
    if (token!=""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"return_cmd\":\""+return_cmd+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
        
        switch(strType){
            case "login":
            case "friend_list":
                break;
            default:
                myMap["K"+msg_id]=new C_Msg(msg_id,strLine);
                break;
        }
        
        log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                +msg+"<br><br>"+log_msg;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
            s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
        
        s_ui.Web_Content("web",css_head+log_msg);
        
        s_tcp.send("m:<s>:"+strLine+":</s>");
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }   
}



function resend_chat_msg(data) {
    for(var key in myMap){
        var pMsg=myMap[key];
        pMsg.Count+=1;
        if (pMsg.Count<10){
            ;
        }else if (pMsg.Count==5){//再发送一次
            log_msg+="<b>resend:"+pMsg.Msg+"</b>";
            s_ui.Web_Content("web",css_head+log_msg);
            s_tcp.send("m:<s>:"+pMsg.Msg+":</s>");
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息发送失败) </font> <span style='color:gray;'>"
                    +obj.id+"="+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息发送失败："+obj.id+"="+obj.message+"\r\n");
            delete myMap["K"+obj.id];
        }
    }
}

function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    
    s_ui.status_label_show("status_label","event_connected!");
    s_ui.text_set("txt_info","event_connected");
    s_ui.button_enable("btn_connect","0");
    
    
    send_msg("login","","","login");
}


function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}

function friend_change(data){
    
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        //s_sys.value_save("friend_selected",friend);
        s_file.Ini_Save(disk+"\\Net\\Web\\main.ini","main","friend_selected",friend);
    }
}

function select_old_friend(data){
    var friend=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","friend_selected");

    if (friend!=null && friend!=""){
        s_ui.text_set("txt_info","选择刚才选择的好友:"+friend);
        s_ui.listbox_select("list_friend",friend);
    }
}


function log_click(data){
    s_ui.Run_App(disk+"\\Net\\Web\\log","");
}


function switch_click(data){
    s_ui.Run_JS("加密聊天_login.js");
}

function chat2(data){
    s_ui.Run_JS("加密聊天.js");
}


function set_click(data){
    s_ui.Run_JS("Chat\\setting.js");
}

function cmd_sub(data){
    s_ui.text_set("txt_send",data);
    //send_msg_click();
}

function show_user(data){
    sep=2;//分隔符用第2个方案
    head="<tr><th>用户名</th><th>口令</th><th>用户ID</th><th>组ID</th><th>注释性描述</th><th>主目录</th><th>登录Shell</th></tr>";
    cmd_sub(data);
}

function show_ps(data){
    sep=1;
    head="<tr><th>USER</th><th>PID</th><th>%CPU</th><th>%MEM</th><th>VSZ</th><th>RSS</th><th>TTY</th><th>STAT</th><th>TART</th><th>TIME</th><th>COMMAND</th></tr>";
    cmd_sub(data);
}

function show_group(data){
    sep=2;//分隔符用第2个方案
    head="<tr><th>用户组名</th><th>用户组密码</th><th>GID</th><th>用户列表</th></tr>";
    cmd_sub(data);
}

function show_file(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux2\\ls.js","callback_cmd");
}


function change_group(data){
    s_sys.value_save("file",data);
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux2\\change_own.js","callback_cmd");
}


function edit_file(data){
    s_sys.value_save("file",data);
    s_sys.value_save("cmd","");
    s_ui.Run_JS("Linux2\\edit_file.js");
}


function callback_cmd(data){
    var line=s_sys.value_read("cmd");
    if (line!=""){
        s_ui.text_set("txt_send",line);
        send_msg_click();
    }
}

function new_user(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\new_user.js","callback_cmd");
}

function new_group(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\new_group.js","callback_cmd");
}

function add_user_2_group(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\user_group.js","callback_cmd");
}

function on_load(){
    var a=sys_read_ini();
    userName=a+"/ai";
    s_ui.text_set("txt_user_name",userName);
}

function event_tcp_error(data){
    s_ui.msg(data);
}


function connect_click(data){
    s_tcp.connect("robot6.funnyai.com",6000,userName,
    "event_connected","event_msg","event_tcp_error");
}



//检查是否联网
function check_connected(data){
    s_ui.text_set("txt_info","check_connected...");
    s_time.setTimeout("check_connected",2,"check_connected");
    
    if (friend_return==1){
        select_old_friend("");
    }
    
    //检查消息是否都发送过去了，没有发送的，再发送一次。
    resend_chat_msg("");
}


function restart_ssh(data){
    data="sudo service ssh restart";
    cmd_sub(data);
}

function file_sql_input(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\file_sql_input.js","callback_cmd");
}


function process_kill(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\process_kill.js","callback_cmd");
}

//s_sys.tcp_event();


s_ui.splitcontainer_init("split",0,0,500,500,"v");
s_ui.splitcontainer_distance("split",100);

s_ui.button_init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");

s_ui.listbox_init("list_friend",10,60,200,180);
s_ui.listbox_init_event("list_friend","friend_change");


s_ui.button_init("b_clear","清空",250,30,450,30,"clear_click","");

s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.text_init("txt_send","hi",380,350,200,30);


s_ui.button_init("b1_send","发送",10,400,100,30,"send_msg_click","");
s_ui.button_init("b1_send2","发送2",10,400,100,30,"send_msg_click2","");

//状态栏 开始
s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");
//状态栏 结束


s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,90,30,"connect_click","");
s_ui.text_init("txt_session","000",10,500,200,30);



s_ui.textbox_init("txt_info","",10,250,200,80);


s_ui.splitcontainer_add("split",0,"list_friend","fill");
s_ui.splitcontainer_add("split",0,"btn_friend","top");
s_ui.splitcontainer_add("split",0,"txt_info","bottom");
s_ui.splitcontainer_add("split",0,"txt_user_name","bottom");
s_ui.splitcontainer_add("split",0,"btn_connect","bottom");


s_ui.splitcontainer_add("split",1,"web","fill");
s_ui.splitcontainer_add("split",1,"b_clear","top");



s_ui.panel_init("panel_top",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel_top","bottom");
s_ui.panel_add("panel_top","txt_send","fill");

s_ui.panel_add("panel_top","b1_send","right");
s_ui.panel_add("panel_top","b1_send2","right");


s_ui.panel_init("panel2",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");

s_ui.panel_init("panel3",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel3","bottom");


s_ui.button_init("btn_hi","hi",10,30,100,30,"cmd_sub","hi");
s_ui.panel_add("panel2","btn_hi","left");
s_ui.button_init("btn_weather","上海天气",10,30,100,30,"cmd_sub","上海天气");
s_ui.panel_add("panel2","btn_weather","left");
s_ui.button_init("btn_weather2","上海的天气",10,30,100,30,"cmd_sub","上海的天气");
s_ui.panel_add("panel2","btn_weather2","left");
s_ui.button_init("btn_shanghai_qh","上海的区号",10,30,100,30,"cmd_sub","上海的区号");
s_ui.panel_add("panel2","btn_shanghai_qh","left");


s_ui.button_init("btn_shimian","什么可以治疗失眠",10,30,150,30,"cmd_sub","什么可以治疗失眠");
s_ui.panel_add("panel3","btn_shimian","left");
s_ui.button_init("btn_quhao_010","什么城市的区号是010",10,30,150,30,"cmd_sub","什么城市的区号是010");
s_ui.panel_add("panel3","btn_quhao_010","left");

s_ui.menu_init("Menu1");//,0,0,800,25);
s_ui.menu_add("Menu1","File","&File");
s_ui.menu_item_add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.menu_item_add("Menu1","File","Chat2","加密聊天","chat2","");
s_ui.menu_add("Menu1","Tools","&Tools");
s_ui.menu_item_add("Menu1","Tools","Setting","设置(&S)","set_click","");


//其他属性
s_ui.button_default("b1_send");
s_ui.show_form(800,600);
s_ui.Form_Title("FunnyAI POST");

on_load("");