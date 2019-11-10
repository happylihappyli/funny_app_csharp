var friend_return=0;
//var keep_count=0;
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
        
            send_msg(obj.id,"chat_return",obj.from,"","");
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

    send_msg("","friend_list","","","friend_list");
}

//发送消息
function send_msg_click(){
    msg_id+=1;
    
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.msg("请选择好友！");
        return ;
    }
    
    var strMsg=s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");

    var strSplit=strMsg.split(" ");
    switch(strSplit[0]){
        case "edit":
            log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                    +strMsg+"<br><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
        
            edit_file(strSplit[1]);
            break;
        case "ai":
            send_msg("","ai",friend,strSplit[1],"");
            break;
        default:
            send_msg("","cmd",friend,strMsg,"");
            break;
    }
    
    s_ui.text_set("txt_send","");
}


function send_msg(oid,strType,friend,msg,return_cmd){
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
    
    
    send_msg("","login","","","login");
}


function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}

function friend_change(data){
    
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        var file_ini=disk+"\\Net\\Web\\main.ini";
        s_file.Ini_Save(file_ini,"main","friend_selected",friend);
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
    send_msg_click();
}

function cmd_text(data){
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

function show_conda_create(data){
    s_sys.value_save("cmd","");
    s_ui.Run_JS_Dialog("Linux2\\conda_create.js","callback_cmd");
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
    userName=a+"/linux";
    s_ui.text_set("txt_user_name",userName);
}

function connect_click(data){
    s_tcp.connect("robot6.funnyai.com",6000,userName,
    "event_connected","event_msg","");
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

s_sys.tcp_event();

[[[ui.js]]]