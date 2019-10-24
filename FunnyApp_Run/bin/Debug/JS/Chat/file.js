
var userName="none";
var md5="";
var log_msg="";
var file_log="E:\\debug.log";
var msg_id=0;
var chat_msgs=[];
var session_send=0;
var read_size=1024;

//消息和发送计数器
function C_Msg(ID,Msg,pos){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
    this.pos=pos;
}


//发送消息
function send_msg_click(){
    msg_id+=1;
    
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.msg("请选择好友！");
        return ;
    }

    
    
    var file1=s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");
    
    s_sys.value_save("file1",file1);
    s_sys.value_save("friend",friend);
    
    var strSplit=file1.split("\\");
    var file_short=strSplit[strSplit.length-1];
    
    
    var pos=0;
    var base64=s_file.Bin_Read(file1,pos,read_size);
    send_msg_sub(friend,file_short,pos,base64);
    //s_net.Send_Msg("chat_event",strLine);
    
    //s_time.setTimeout("resend_chat_msg", 3);//检查消息是否都发送过去了，没有发送的，再发送一次。
    
}


function send_msg_sub(friend,file_short,pos,strMsg){
    msg_id+=1;
    
    var strType="file";
    var token="";
    var strLine="";
    
    strLine="{\"id\":\""+msg_id+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"file\":\""+file_short+"\","
            +"\"pos\":\""+pos+"\",\"message\":\""+strMsg+"\"}";
    s_file.append(file_log,strLine);
    
    
    chat_msgs["K"+msg_id]=new C_Msg(msg_id,strLine,pos);
    
    s_net.Send_Msg("chat_event",strLine);
}



function resend_chat_msg(data) {
    var mycount=0;
    var html="";
    for(var key in chat_msgs){
        mycount+=1;
        var pMsg=chat_msgs[key];
        html+=key+",";
        if (pMsg.Count<30){
            pMsg.Count+=1;
            var index=(pMsg.pos/read_size);
            s_ui.text_set("txt_info3","key:"+key+",pos="+index+",Count:"+pMsg.Count);
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
    s_ui.text_set("txt_info4","resend_chat_msg:"+data+" chat_msgs.count="+mycount);
    if (mycount>1){
        s_ui.Web_Content("web",html);
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
    
    
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_ui.button_enable("btn_connect",1);
    //s_net.Socket_Connect();
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
        case "file":
            s_ui.text_set("txt_info","file:"+obj.pos);
            var strMsg=s_time.Time_Now()+" "+obj.message;
            
            var file=obj.file;
            var file2="E:\\"+file;
            if (obj.pos=="0"){
                s_file.delete(file2);
            }
            s_file.Bin_Write(file2,obj.pos,obj.message);
            
            msg_id+=1;
            var id=obj.id;
            var strLine="{\"id\":\""+msg_id+"\","
            +"\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"old_id\":\""+id+"\",\"pos\":\""+obj.pos+"\"}";
            s_file.append(file_log,strLine);

            s_net.Send_Msg("sys_event",strLine); //消息返回，告诉对方，收到了
            
            break;
        default:
            break;
    }
}

function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.text_set("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            var index=obj.pos/read_size;
            s_ui.text_set("txt_info2","chat_return id="+obj.old_id+" .pos:"+index);
            delete chat_msgs["K"+obj.old_id];
            
            var file1=s_sys.value_read("file1");
            var friend=s_sys.value_read("friend");
            var strSplit=file1.split("\\");
            var file_short=strSplit[strSplit.length-1];

            
            var size=s_file.Size(file1);
            
            var pos=parseInt(obj.pos)+read_size;
            var base64=s_file.Bin_Read(file1,pos,read_size);
            
            send_msg_sub(friend,file_short,pos,base64);
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.text_set("txt_session",session_id);  
                s_ui.text_set("txt_user_name",userName);
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
                session_send=1;
                
                friend_list();
            }
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=s_sys.Path_App();
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

s_ui.text_init("txt_send","E:\\CloudStation\\test.pdf",250,350,450,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");


s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,100,30,"connect_click","");
s_ui.text_init("txt_session","000",10,500,200,30);


s_ui.text_init("txt_info","",250,450,450,30);
s_ui.text_init("txt_info2","",250,480,450,30);
s_ui.text_init("txt_info3","",250,510,450,30);
s_ui.text_init("txt_info4","",250,540,450,30);


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
s_ui.Show_Form(800,700);
s_ui.Form_Title("File");

//s_ui.ShowInTask(0);
connect_click("");

check_connected("");