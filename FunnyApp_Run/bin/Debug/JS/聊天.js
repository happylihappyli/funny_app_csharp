
var userName="none";
var log_msg="";

//发送消息
function send_msg_click(){
    
    var strMsg=userName+":"+sys.Get_Text("txt_send");
    var friend=sys.ListBox_Text("list_friend");
    var index=sys.ListBox_Index("list_friend");
    if (index<0){
        sys.Msg("请选择好友！");
        return ;
    }
    

    //var friend="*";
    var strLine="{\"from\":\""+userName+"\",\"type\":\"encrypt\",\"to\":\""
    +friend+"\",\"message\":\""+strMsg+"\"}";
    sys.Send_Msg("chat_event",strLine);
    
    
    log_msg=sys.Time_Now()+" "+strMsg+"\r\n\r\n"+log_msg;
    sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",
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
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    if (obj.type=="encrypt"){
    }else{
        var strMsg=sys.Time_Now()+" "+obj.message;
        sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
        log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
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
    sys.Combox_Clear("cb_friend");
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
    sys.Init_Socket(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
    sys.Button_Enable("btn_connect",0);
}

function log_click(data){
    sys.Run_App("D:\\Net\\Web\\log","");
}


function switch_click(data){
    sys.Run_JS("加密聊天_login.js");
}

function chat2(data){
    sys.Run_JS("加密聊天.js");
}

function friend_list(data){
    sys.ListBox_Clear("list_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    sys.Send_Msg("sys_event",strLine);
}



sys.Button_Init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");
//sys.Label_Init("lb_txt1","好友：",10,30);
sys.ListBox_Init("list_friend",10,60,200,380);


sys.Button_Init("b_clear","清空聊天记录",250,30,450,30,"clear_click","");

sys.TextBox_Init("txt1","接收到信息",250,60,450,250);

sys.Add_Text("txt_send","hi",250,350,450,30);
sys.Button_Init("b1_send","发送",600,400,100,30,"send_msg_click","");



sys.Add_Text("txt_user_name","000",10,450,100,30);
sys.Button_Init("btn_connect","连服务器",120,450,100,30,"connect_click","");
sys.Add_Text("txt_session","000",10,500,200,30);



sys.TextBox_Init("txt_info","",250,450,450,80);


sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","&File");
sys.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
sys.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");



//其他属性
sys.Acception_Button("b1_send");
sys.Show_Form(800,600);
sys.Form_Title("聊天");

//sys.ShowInTask(0);
connect_click("");