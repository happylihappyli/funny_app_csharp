
var userName="none";
var log_msg="";

function send_msg_click(){
    
    var strMsg=userName+":"+sys.Get_Text("txt_send");
    var friend=sys.Combox_Text("cb_friend");
    var index=sys.Combox_Index("cb_friend");
    if (index<0){
        sys.Msg("请选择好友！");
        return ;
    }
    
    var path=sys.AppPath();
    var file=sys.Ini_Read(path+"\\config\\friend.ini","item"+index,"file");
    
    if (sys.File_Exists(file)==false){
        sys.Msg("公钥不存在:"+file);
        return ;
    }
    var strLine=sys.encrypt_public_key(file,strMsg);
    sys.Show_Text("txt_send_en",strLine);
    var friend=sys.Combox_Text("cb_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"encrypt\",\"to\":\""
    +friend+"\",\"message\":\""+strLine+"\"}";
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
    log_msg+="connected\r\n";
    sys.Show_Text("txt1",log_msg);
}

function clear_click(data){
    log_msg="clear\r\n";
    sys.Show_Text("txt1",log_msg);
}


function event_chat(data){
    var friend=sys.Combox_Text("cb_friend");
    var obj=JSON.parse(data);
    
    if (obj.type=="encrypt"){
        if (obj.to==userName){
            var strMsg=sys.Time_Now()+" "+sys.decrypt_private_key("D:/Net/Web/id_rsa",obj.message);
            sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
            log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
        }else{
            //log_msg="to="+obj.to+"\r\n"+"\r\n"+log_msg;
        }
    }else{
        //var strMsg=sys.Time_Now()+" "+obj.message;
        //sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
        //log_msg=strMsg+"\r\n"+"\r\n"+log_msg;
    }
    
    sys.Show_Text("txt1",log_msg);
    
    sys.Notification(obj.from,strMsg);
}

function event_system(data){
    var obj=JSON.parse(data);
    log_msg+=obj.from+"："+obj.message+"\r\n";
    sys.Show_Text("txt1",log_msg);
    switch(obj.type){
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                sys.Show_Text("txt_session",session_id);  
                sys.Show_Text("txt_user_name",userName); 
                //$("#user_name").html(userName);
                sys.Send_Msg("sys_event",userName, "*_session", session_id); //服务器会记录用户名
            }
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
        sys.Combox_Add("cb_friend",strName);
    }
    if (count>0){
        sys.Combox_Select("cb_friend",0);
    }
}


function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    sys.Connect_Socket(url,"event_connected","event_chat","event_system");
    read_ini();
}

function log_click(data){
    sys.Run_App("D:\\Net\\Web\\log","");
}


function switch_click(){
    sys.Run_JS("加密聊天_login.js");
}

sys.TextBox_Init("txt1","接收到信息",10,10,300,300);


sys.Add_Text("txt_send","hi",10,350,300,30);
//sys.Text_KeyDown("txt_send","text_keydown");

sys.Label_Init("lb_txt1","好友：",10,450);
sys.Add_Combox("cb_friend","",100,450,200,30);


sys.Button_Init("b2","clear",200,400,100,30,"clear_click","");

sys.Add_Text("txt_user_name","000",350,250,100,30);
sys.Button_Init("btn_connect","连服务器",480,240,100,30,"connect_click","");
sys.Button_Init("btn_connect","log",600,240,100,30,"log_click","");
sys.Add_Text("txt_session","000",350,300,300,30);

sys.Button_Init("b1_send","发送",10,400,100,30,"send_msg_click","");
sys.Acception_Button("b1_send");
sys.TextBox_Init("txt_send_en","",350,10,300,150);

sys.Show_Form(800,600);
sys.Form_Title("加密聊天");
