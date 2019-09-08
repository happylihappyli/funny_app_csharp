
function send_msg_click(){
    sys.Send_Msg("csharp","*",s_ui.Text_Read("send_msg"));
}


function event_connected(data){
    s_ui.Text_Set("txt1","Connected");
}


function event_chat(data){
    var obj=JSON.parse(data);
    s_ui.Text_Set("txt1",obj.message);
}



s_ui.Button_Init("b1","发送",10,100,200,30,"send_msg_click","");

s_ui.Text_Init("txt1","接收到信息",10,10,200,30);
s_ui.Text_Init("send_msg","",10,50,200,50);

sys.Socket_Init("http://robot6.funnyai.com:8000","event_connected","event_chat","","");

s_ui.Show_Form(300,300);

s_ui.Form_Title("socket.io test");


