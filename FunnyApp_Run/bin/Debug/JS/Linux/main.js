var disk="D:";
var userName="none";
var md5="";
var log_msg="";
var session_send=0;
var msg_id=0;
var myMap=[];
var head="";
var css_head='<html><head>\n'
+'<link href="http://www.funnyai.com/Common/css/default.css" type="text/css" rel="stylesheet" />\n'
+'<link href="http://www.funnyai.com/Common/css/table.css" type="text/css" rel="stylesheet" />\n'
+'<body>\n';

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}

var sep=1;



function New_URL(data){
    s_ui.Msg(data);
    var strSplit=data.split("?");
    var file=strSplit[1];
    switch(strSplit[0]){
        case "http://file.edit/":
            var file2=file.substr(5);
            change_group(file2);
            break;
    }
}

[[[event_chat.js]]]


//发送消息
function send_msg_click(){
    msg_id+=1;
    
    var index=s_ui.ListBox_Index("list_friend");
    if (index<0){
        s_ui.Msg("请选择好友！");
        return ;
    }
    var strMsg=s_ui.Combox_Text("combox_head")+" "+s_ui.Text_Read("txt_send");
    var friend=s_ui.ListBox_Text("list_friend");
    var strType="js";
    
    var token="";
    
    var url="http://www.funnyai.com/login_get_token_json.php";
    var name=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    var md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    
    var result=s_net.http_post(url,data);
    if (result.indexOf("登录成功")>-1){
        var strSplit=result.split("=");
        token=strSplit[2];
    }


    var strLine="";
    
    if (token==""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg+"\"}";
    }else{
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg+"\"}";
    }
    
    s_ui.Text_Set("txt_info",strLine);
    
    myMap["K"+msg_id]=new C_Msg(msg_id,strLine);
    
    s_net.Send_Msg("chat_event",strLine);
    
    
    log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
            +strMsg+"<br><br>"+log_msg;
    s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
        s_time.Date_Now()+" "+s_time.Time_Now()+" "+strMsg+"\r\n");
    
    s_ui.Web_Content("web",css_head+log_msg);
    s_ui.Text_Set("txt_send","");
    
}

function resend_chat_msg(data) {
    for(var key in myMap){
        var pMsg=myMap[key];
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息丢失："+obj.message+"\r\n");
                
            
            s_ui.Web_Content("web",css_head+log_msg);
        }
    }
}

function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    s_ui.Text_Set("txt_info","event_connected");
    s_ui.Button_Enable("btn_connect",0);
    friend_list();
    
}

function event_disconnected(data){
    s_ui.Text_Set("txt_info","event_disconnected");
    s_ui.Button_Enable("btn_connect",1);
    s_net.Socket_Connect();
}

function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}

function friend_change(data){
    
    var friend=s_ui.ListBox_Text("list_friend");
    if (friend!=""){
        //s_sys.Value_Save("friend_selected",friend);
        s_file.Ini_Save(disk+"\\Net\\Web\\main.ini","main","friend_selected",friend);
    }
}

function select_old_friend(data){
    var friend=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","friend_selected");

    if (friend!=null && friend!=""){
        s_ui.Text_Set("txt_info","选择刚才选择的好友:"+friend);
        s_ui.ListBox_Select("list_friend",friend);
    }
}


function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.Text_Set("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            s_ui.Text_Set("txt_info","chat_return:"+obj.message);
            delete myMap["K"+obj.oid];
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.Text_Set("txt_session",session_id);  
                s_ui.Text_Set("txt_user_name",userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
                session_send=1;
            }
            break;
        case "list.all":
            s_ui.ListBox_Add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=sys.AppPath();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    userName=userName2+"/linux";
    
}

function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
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

function friend_list(data){
    s_ui.ListBox_Clear("list_friend");
    s_ui.ListBox_Add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}

function set_click(data){
    s_ui.Run_JS("Chat\\setting.js");
}

function cmd_sub(data){
    s_ui.Text_Set("txt_send",data);
    send_msg_click();
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
    sep="file_list";//分隔符用第1.1个方案
    head="<tr><th>权限</th><th>子目录(文件)数</th><th>所属用户</th><th>所属用户组</th><th>大小</th><th colspan=3>最近修改/查看时间</th><th>名称</th></tr>";
    cmd_sub(data);  
}


function change_group(data){
    s_sys.Value_Save("file",data);
    s_sys.Value_Save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\change_own.js","callback_cmd");
}


function callback_cmd(data){
    var line=s_sys.Value_Read("cmd");
    if (line!=""){
        s_ui.Text_Set("txt_send",line);
        send_msg_click();
    }
}

function new_user(data){
    s_sys.Value_Save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\new_user.js","callback_cmd");
}

function new_group(data){
    s_sys.Value_Save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\new_group.js","callback_cmd");
}

function add_user_2_group(data){
    s_sys.Value_Save("cmd","");
    s_ui.Run_JS_Dialog("Linux\\user_group.js","callback_cmd");
}

//检查是否联网
function check_connected(data){
    s_ui.Text_Set("txt_info","check_connected...");
    s_time.setTimeout("check_connected",2,"check_connected");
    
    if (s_net.Socket_Connected()){
        //friend_list("");
        
        if (session_send==1){
            select_old_friend("");
            //检查消息是否都发送过去了，没有发送的，再发送一次。
            resend_chat_msg("");
        }
    }else{
        session_send=0;
        s_net.Socket_Connect();//如果没有连，会自动连
    }
}

function restart_ssh(data){
    data="sudo service ssh restart";
    cmd_sub(data);
}

[[[ui.js]]]