
var userName="none";
var log_msg="";

function send_msg_click(){
    
    var strMsg=userName+":"+sys.Get_Text("txt_send");
    var friend=sys.ListBox_Text("list_friend");
    var index=sys.ListBox_Index("list_friend");
    if (index<0){
        sys.Msg("请选择好友！");
        return ;
    }
    
    var path=sys.AppPath();
    var count=parseInt(sys.Ini_Read(path+"\\config\\friend.ini","items","count"));
    var file="";
    var name="";
    var iFind=0;
    for (var i=0;i<count;i++){
        name=sys.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        file=sys.Ini_Read(path+"\\config\\friend.ini","item"+i,"file");
        if (name==friend){
            iFind=1;
            break;
        }
    }
    
    if (iFind==0 || sys.File_Exists(file)==false){
        sys.Msg("公钥不存在:"+file);
        return ;
    }
    var strLine=sys.encrypt_public_key(file,strMsg);
    sys.Show_Text("txt_send_en",strLine);
    var friend=sys.ListBox_Text("list_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"encrypt\",\"to\":\""
    +friend+"\",\"message\":\""+strLine+"\"}";
    sys.Send_Msg("chat_event",strLine);
    
    
    log_msg=sys.Time_Now()+" "+strMsg+"\r\n\r\n"+log_msg;
    sys.File_Append("C:\\Net\\Web\\log\\"+friend+".txt",
        sys.Date_Now()+" "+sys.Time_Now()+" "+strMsg+"\r\n");
    sys.Show_Text("txt1",log_msg);
    sys.Show_Text("txt_send","");
}


function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    sys.Show_Text("txt_info","event_connected");
    friend_list();
}

function event_disconnected(data){
    sys.Show_Text("txt_info","event_disconnected");
    sys.Socket_Connect();
}


function clear_click(data){
    log_msg="clear\r\n";
    sys.Show_Text("txt1",log_msg);
}


function event_chat(data){
    var friend=sys.ListBox_Text("list_friend");
    var obj=JSON.parse(data);
    
    if (obj.type=="encrypt"){
        if (obj.to==userName){
            var strMsg=sys.Time_Now()+" "+sys.decrypt_private_key("C:/Net/Web/id_rsa",obj.message);
            sys.File_Append("C:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
            log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
        }else{
            //log_msg="to="+obj.to+"\r\n"+"\r\n"+log_msg;
        }
    }else{
        //var strMsg=sys.Time_Now()+" "+obj.message;
        //sys.File_Append("C:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
        //log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
    }
    
    sys.Show_Text("txt1",log_msg);
    
    sys.Notification(obj.from,strMsg);
}

function event_system(data){
    
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    sys.Show_Text("txt_info",log_msg2);
    switch(obj.type){
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                sys.Show_Text("txt_session",session_id);  
                sys.Show_Text("txt_user_name",userName); 
                //$("#user_name").html(userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                sys.Send_Msg("sys_event",strLine); //服务器会记录用户名
            }
            break;
        case "list.all":
            sys.ListBox_Add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    var path=sys.AppPath();
    var strCount=sys.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=sys.Ini_Read(path+"\\config\\friend.ini","main","account");
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=sys.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //sys.Combox_Add("cb_friend",strName);
    }
    if (count>0){
        //sys.Combox_Select("cb_friend",0);
    }
}


function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    sys.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
    sys.Button_Enable("btn_connect",0);
}

function log_click(data){
    sys.Run_App("C:\\Net\\Web\\log","");
}


function switch_click(){
    sys.Run_JS("加密聊天_login.js");
}


function friend_list(data){
    sys.ListBox_Clear("list_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    sys.Send_Msg("sys_event",strLine);
}

sys.Button_Init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");
sys.ListBox_Init("list_friend",10,60,200,380);


sys.Button_Init("b_clear","清空聊天记录",250,30,450,30,"clear_click","");
sys.TextBox_Init("txt1","接收到信息",250,60,450,200);

sys.Add_Text("txt_send","hi",250,350,450,30);

sys.Button_Init("b_send","发送",450,400,100,30,"send_msg_click","");



sys.Add_Text("txt_user_name","000",10,450,100,30);
sys.Button_Init("btn_connect","连服务器",120,450,100,30,"connect_click","");
sys.Add_Text("txt_session","000",10,500,200,30);




sys.TextBox_Init("txt_send_en","",750,60,300,150);

sys.TextBox_Init("txt_info","",250,450,450,80);


//菜单
sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","&File");
sys.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
sys.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");

//窗口
sys.Acception_Button("b_send");
sys.Show_Form(750,600);
sys.Form_Title("加密聊天");

connect_click("");