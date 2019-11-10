var userName="test";
var disk="D:";


[[[..\\data\\common_string.js]]]
[[[..\\data\\default.js]]]
[[[..\\data\\tcp.js]]]



function on_load(){
    var userName2=sys_read_ini();
    userName=userName2+"/tcp";
}


function send_msg_click(data){
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.status_label_show("status_label","请选择好友！!");
        //s_ui.msg("请选择好友！");
        return ;
    }
    var strMsg=s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");
    var strType="cmd";
    send_msg(strType,friend,strMsg,"");
    s_ui.text_set("txt_send","");
    
}


var log_msg="";
var keep_count=1;
function event_msg(data){
    var obj=JSON.parse(data);
    var msg=obj.message;
    switch(obj.type){
        case "chat_return":
            //s_ui.msg(json);
            delete myMap["K"+obj.oid];
            s_ui.text_set("tx_status",msg);
            break;
        case "login.ok":
            friend_list("");
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            friend_return=1;
            break;
        default:
            msg=msg.replaceAll("\n","\r\n");
            log_msg=msg+"\r\n"+log_msg;
            s_ui.text_set("txt1",log_msg);
            break;
    }
}

function event_connected(data){
    log_msg+="connected!";
    s_ui.text_set("txt1",log_msg);
    
    send_msg("login","","","login");
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
        
        
        //s_ui.Web_Content("web",css_head+log_msg);
        
        s_tcp.send("m:<s>:"+strLine+":</s>");
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }   
}


function friend_list(data){
    
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");

    send_msg("friend_list","","","friend_list");
}



function event_tcp_error(data){
    s_ui.msg(data);
}


function connect_click(data){
    
    s_tcp.connect("robot6.funnyai.com",6000,userName,
    "event_connected","event_msg","event_tcp_error");
}

function clear_click(data){
    log_msg="";
    s_ui.text_set("txt1",log_msg);
}


function friend_change(data){
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        s_file.Ini_Save(disk+"\\Net\\Web\\main.ini","main","friend_selected",friend);
    }
}


s_ui.button_init("b_send","发送",250,50,100,30,"send_msg_click","");
s_ui.button_init("b_clear","Clear",350,50,100,30,"clear_click","");

s_ui.text_init("txt_send","ls",10,50,200,50);

s_ui.text_init("tx_status","",10,100,200,50);
s_ui.button_init("b_connect","连接",250,100,100,30,"connect_click","");
s_ui.button_init("b_friend_list","friend_list",350,100,100,30,"friend_list","");


s_ui.listbox_init("list_friend",10,150,200,300);
s_ui.listbox_init_event("list_friend","friend_change");

s_ui.textbox_init("txt1","显示信息：",250,150,300,300);

//状态栏 开始
s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");
//状态栏 结束


s_ui.button_default("b_send");

s_ui.show_form(600,600);
s_ui.Form_Title("TCP");

s_sys.tcp_event();

on_load("");



