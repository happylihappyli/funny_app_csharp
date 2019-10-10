
var userName="none";
var md5="";
var log_msg="";

var msg_id=0;
var chat_msgs=[];
var session_send=0;

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
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
    var strType="msg";// s_ui.combox_text("combox1");
    
    var token="";

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
    
    s_ui.text_set("txt_info",strLine);
    
    chat_msgs[msg_id]=new C_Msg(msg_id,strLine);
    
    s_net.Send_Msg("chat_event",strLine);
    
    
    log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
            +strMsg+"<br><br>"+log_msg;
    s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",
        s_time.Date_Now()+" "+s_time.Time_Now()+" "+strMsg+"\r\n");
        
    s_ui.Web_Content("web",log_msg);
    s_ui.text_set("txt_send","");
    
    //s_time.setTimeout("resend_chat_msg", 3);//检查消息是否都发送过去了，没有发送的，再发送一次。
    
}

function resend_chat_msg(data) {
    for(var key in chat_msgs){
        var pMsg=chat_msgs[key];
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息丢失："+obj.message+"\r\n");
                
            s_ui.Web_Content("web",log_msg);
        }
    }
}

function friend_change(data){
    
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        s_sys.value_save("friend_selected",friend);
    }
}

function select_old_friend(data){
    var friend=s_sys.value_read("friend_selected");
    if (friend!=null && friend!=""){
        s_ui.text_set("txt_info","选择刚才选择的好友:"+friend);
        s_ui.listbox_select("list_friend",friend);
    }
}



function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    s_ui.text_set("txt_info","event_connected");
    s_ui.button_enable("btn_connect",0);
    friend_list();
    
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_ui.button_enable("btn_connect",1);
    s_net.Socket_Connect();
}

function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}


function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    switch (obj.type){
        case "encrypt":
            break;
        case "msg":
            var index=s_ui.listbox_select("list_friend",friend);
            if (index==-1){
                friend_list("");
                //sys.sleep(10);
                s_ui.listbox_select("list_friend",friend);
            }
            var strMsg=s_time.Time_Now()+" "+obj.message;
            s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
            log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span> <font color=blue><br>"
            +obj.message+"</font><br><br>\r\n"+log_msg;
            
            var id=obj.id;
            var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"message\":\""+id+"\"}";
            s_net.Send_Msg("sys_event",strLine); //消息返回
            break;
        default:
            break;
    }
    
    s_ui.Web_Content("web",log_msg);
    
    s_ui.Notification(obj.from,obj.message);
}

function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.text_set("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            s_ui.text_set("txt_info","chat_return:"+obj.message);
            delete chat_msgs[obj.message];
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.text_set("txt_session",session_id);  
                s_ui.text_set("txt_user_name",userName);
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
                session_send=1;
            }
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=s_sys.AppPath();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=s_file.Ini_Read("D:\\Net\\Web\\main.ini","main","account");
    md5=s_file.Ini_Read("D:\\Net\\Web\\main.ini","main","md5");
    userName=userName2+"/public";
    
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=s_file.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //s_ui.combox_add("cb_friend",strName);
    }
    if (count>0){
        //s_ui.Combox_Select("cb_friend",0);
    }
}

function connect_click(data){
    read_ini();
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
}

function log_click(data){
    s_ui.Run_App("D:\\Net\\Web\\log","");
}


function switch_click(data){
    s_ui.Run_JS("加密聊天_login.js");
}

function chat2(data){
    s_ui.Run_JS("加密聊天.js");
}

function friend_list(data){
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}

function set_click(data){
    s_ui.Run_JS("Chat\\setting.js");
}

function linux_click(data){
    s_ui.Run_JS("Linux\\main.js");
}

function file_click(data){
    s_ui.Run_JS("Chat\\file.js");
    
}


//检查是否联网
function check_connected(data){
    s_ui.text_set("txt_info","check_connected...");
    s_time.setTimeout("check_connected",5,"check_connected");
    
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



s_ui.button_init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");

s_ui.listbox_init("list_friend",10,60,200,380);
s_ui.listbox_init_event("list_friend","friend_change");


s_ui.button_init("b_clear","清空聊天记录",250,30,450,30,"clear_click","");

s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");

s_ui.text_init("txt_send","hi",250,350,450,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,100,30,"connect_click","");
s_ui.text_init("txt_session","000",10,500,200,30);



s_ui.text_init("txt_info","",250,450,450,30);
s_ui.text_init("txt_info2","",250,500,450,30);


s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");
s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Linux","Linux","linux_click","");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&S)","set_click","");
s_ui.Menu_Item_Add("Menu1","Tools","File","文件(&S)","file_click","");



//其他属性
s_ui.button_default("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("聊天");

//s_ui.ShowInTask(0);
connect_click("");

check_connected("");