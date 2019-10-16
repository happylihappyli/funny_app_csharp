var userName="test";
var disk="D:";

[[[..\\data\\common_string.js]]]
[[[..\\data\\default.js]]]

var msg_id=1;
var myMap=[];

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}

function read_ini(){
    var path=s_sys.AppPath();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    userName=userName2+"/tcp";
    
}


function send_msg_click(data){
    var msg=s_ui.text_read("send_msg");
    
    var strType="cmd";
    var strLine="";
    var friend=s_ui.listbox_text("list_friend");
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    var token=get_token();
    msg_id+=1;
    strLine="{\"id\":\""+msg_id+"\","
        +"\"token\":\""+token+"\","
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
    
    s_tcp.send("m:<s>:"+strLine+":</s>");
}


var log_msg="";
var keep_count=1;
function event_msg(data){
    data=data.replaceAll("\r\n","\n");
    var strSplit=data.split("\n");
    if (strSplit[0]=="s:keep"){
        s_ui.text_set("tx_status","keep"+keep_count);
        keep_count++;
    }else if(strSplit[0].indexOf("m:<s>:")==0){
        show_msg(data);
    }else{
        log_msg=data+"\r\n"+log_msg;
        s_ui.text_set("txt1",log_msg);
    }
}

function show_msg(data){
    var index1=data.indexOf(":<s>:");
    var index2=data.indexOf(":</s>");
    if (index2>index1 && index1>0){
        while(index2>index1 && index1>0){
            var json=data.substring(index1+5,index2);
            var obj=JSON.parse(json);
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
                    break;
                default:
                    msg=msg.replaceAll("\n","\r\n");
                    log_msg=msg+"\r\n"+log_msg;
                    s_ui.text_set("txt1",log_msg);
                    break;
            }
            data=data.substring(index2+6);
            
            index1=data.indexOf(":<s>:");
            index2=data.indexOf(":</s>");
        }
    }else{
        log_msg=index1+":"+index2+":";//+data+"\r\n"+log_msg;
        s_ui.text_set("txt1",log_msg);
    }
}

function event_connected(data){
    log_msg+="connected!";
    s_ui.text_set("txt1",log_msg);
    
    var friend="*";
    msg_id+=1;
    var token=get_token();
    if (token!=""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"from\":\""+userName+"\",\"type\":\"login\","
            +"\"to\":\""+friend+"\",\"message\":\"\"}";
    }
    
    s_tcp.send("m:<s>:"+strLine+":</s>");
}


function friend_list(data){
    
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");
    var friend="*";
    msg_id+=1;
    var token=get_token();
    if (token!=""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"from\":\""+userName+"\",\"type\":\"friend_list\","
            +"\"to\":\""+friend+"\",\"message\":\"\"}";
    }
    
    s_tcp.send("m:<s>:"+strLine+":</s>");
}

function connect_click(data){
    
    s_tcp.connect("robot6.funnyai.com",6000,userName,"event_connected","event_msg");
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

s_ui.text_init("send_msg","ls",10,50,200,50);

s_ui.text_init("tx_status","",10,100,200,50);
s_ui.button_init("b_connect","连接",250,100,100,30,"connect_click","");
s_ui.button_init("b_friend_list","friend_list",350,100,100,30,"friend_list","");


s_ui.listbox_init("list_friend",10,150,200,300);
s_ui.listbox_init_event("list_friend","friend_change");

s_ui.textbox_init("txt1","显示信息：",250,150,300,300);


s_ui.button_default("b_send");

s_ui.Show_Form(600,500);
s_ui.Form_Title("TCP");

read_ini("");



