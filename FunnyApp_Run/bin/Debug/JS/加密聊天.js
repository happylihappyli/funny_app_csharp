
var userName="happyli";
var log_msg="";

function send_msg_click(){
    
    var strMsg=sys.Get_Text("txt_send");
    var file=sys.Get_Text("txt_file_public");
    var strLine=sys.encrypt_public_key(file,strMsg);
    sys.Show_Text("txt_send_en",strLine);
    sys.Send_Msg("chat_event","encrypt","*",strLine);
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
    var obj=JSON.parse(data);
    
    if (obj.from=="encrypt"){
        log_msg+=">>"+obj.message+"\r\n"+"\r\n";
        var strMsg=sys.decrypt_private_key("D:/Net/Web/id_rsa",obj.message);
        log_msg+=strMsg+"\r\n"+"\r\n";
    }else{
        var strMsg=obj.message;
        log_msg+=strMsg+"\r\n"+"\r\n";
    }
    
    sys.Show_Text("txt1",log_msg);
    
    sys.Notification(obj.from,strMsg);
}

function event_system(data){
    var obj=JSON.parse(data);
    log_msg+=obj.from+"："+obj.message+"\r\n";
    sys.Show_Text("txt1",log_msg);
    switch(obj.from){
        case "system":
            if (obj.to=="30s:session"){
                session_id=obj.message;
                sys.Show_Text("txt_session",session_id);  
                sys.Show_Text("txt_user_name",userName); 
                //$("#user_name").html(userName);
                sys.Send_Msg("sys_event",userName, "*_session", session_id); //服务器会记录用户名
            }
            break;
    }
}



sys.Add_Text_Multi("txt1","接收到信息",10,10,300,300);


sys.Add_Text("txt_send","hi",10,350,300,30);


sys.Add_Label("lb_txt1","好友的公钥：",350,10);
sys.Add_Text("txt_file_public","D:/Net/Web/public/pem/id_rsa_oneyes.pem.pub",350,50,300,30);

sys.Add_Button("b2","clear",200,400,100,30,"clear_click","");

sys.Add_Text("txt_user_name","000",10,450,100,30);
sys.Add_Text("txt_session","000",200,450,300,30);

sys.Add_Button("b1","发送",10,400,100,30,"send_msg_click","");
sys.Add_Text("txt_send_en","",10,500,500,50);

sys.Connect_Socket("http://robot3.funnyai.com:7777","event_connected","event_chat","event_system");

sys.Show_Form(800,600);
sys.Form_Title("加密聊天");


