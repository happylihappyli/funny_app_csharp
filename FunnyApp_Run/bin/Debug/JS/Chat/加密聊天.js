
var userName="none";
var log_msg="";

function send_msg_click(){
    
    var strMsg=userName+":"+s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.msg("请选择好友！");
        return ;
    }
    
    var path=s_sys.AppPath();
    var count=parseInt(s_file.Ini_Read(path+"\\config\\friend.ini","items","count"));
    var file="";
    var name="";
    var iFind=0;
    for (var i=0;i<count;i++){
        name=s_file.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        file=s_file.Ini_Read(path+"\\config\\friend.ini","item"+i,"file");
        if (name==friend){
            iFind=1;
            break;
        }
    }
    
    if (iFind==0 || s_file.File_Exists(file)==false){
        s_ui.msg("公钥不存在:"+file);
        return ;
    }
    var strLine=s_string.encrypt_public_key(file,strMsg);
    s_ui.text_set("txt_send_en",strLine);
    var friend=s_ui.listbox_text("list_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"encrypt\",\"to\":\""
    +friend+"\",\"message\":\""+strLine+"\"}";
    s_net.Send_Msg("chat_event",strLine);
    
    
    log_msg=s_time.Time_Now()+" "+strMsg+"\r\n\r\n"+log_msg;
    s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",
        s_time.Date_Now()+" "+s_time.Time_Now()+" "+strMsg+"\r\n");
    s_ui.text_set("txt1",log_msg);
    s_ui.text_set("txt_send","");
}


function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    s_ui.text_set("txt_info","event_connected");
    friend_list();
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_net.Socket_Connect();
}


function clear_click(data){
    log_msg="clear\r\n";
    s_ui.text_set("txt1",log_msg);
}


function event_chat(data){
    var friend=s_ui.listbox_text("list_friend");
    var obj=JSON.parse(data);
    
    if (obj.type=="encrypt"){
        if (obj.to==userName){
            var strMsg=s_time.Time_Now()+" "+s_string.decrypt_private_key("D:/Net/Web/id_rsa",obj.message);
            s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
            log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
        }else{
            //log_msg="to="+obj.to+"\r\n"+"\r\n"+log_msg;
        }
    }else{
        //var strMsg=s_time.Time_Now()+" "+obj.message;
        //s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
        //log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
    }
    
    s_ui.text_set("txt1",log_msg);
    
    s_ui.Notification(obj.from,strMsg);
}

function event_system(data){
    
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.text_set("txt_info",log_msg2);
    switch(obj.type){
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.text_set("txt_session",session_id);  
                s_ui.text_set("txt_user_name",userName); 
                //$("#user_name").html(userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
            }
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    var path=s_sys.AppPath();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=s_file.Ini_Read(path+"\\config\\friend.ini","main","account");
    
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
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
    s_ui.button_enable("btn_connect",0);
}

function log_click(data){
    s_ui.Run_App("D:\\Net\\Web\\log","");
}


function switch_click(){
    s_ui.Run_JS("加密聊天_login.js");
}


function friend_list(data){
    s_ui.listbox_clear("list_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}

s_ui.button_init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");
s_ui.listbox_init("list_friend",10,60,200,380);


s_ui.button_init("b_clear","清空聊天记录",250,30,450,30,"clear_click","");
s_ui.textbox_init("txt1","接收到信息",250,60,450,200);

s_ui.text_init("txt_send","hi",250,350,450,30);

s_ui.button_init("b_send","发送",450,400,100,30,"send_msg_click","");



s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,100,30,"connect_click","");
s_ui.text_init("txt_session","000",10,500,200,30);




s_ui.textbox_init("txt_send_en","",750,60,300,150);

s_ui.textbox_init("txt_info","",250,450,450,80);


//菜单
s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");

//窗口
s_ui.button_default("b_send");
s_ui.Show_Form(750,600);
s_ui.Form_Title("加密聊天");

connect_click("");