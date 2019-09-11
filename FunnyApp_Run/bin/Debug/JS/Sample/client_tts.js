
function send_msg_click(data){
    s_net.Send_Msg(s_ui.Text_Read("send_msg"));
}


function event_connected(data){
    s_ui.Text_Set("txt1","Connected");
}


function event_chat(data){
    var obj=JSON.parse(data);
    s_ui.Text_Set("txt1",obj.message);
    sys.TTS(obj.message);
    s_ui.Notification(obj.from,obj.message);
}

function event_system(data){
    
}


s_ui.Button_Init("b1","发送",10,100,200,30,"send_msg_click","");

s_ui.Text_Init("txt1","接收到信息",10,10,200,30);
s_ui.Text_Init("send_msg","",10,50,200,50);

s_net.Socket_Init("http://robot6.funnyai.com:8000","event_connected","event_chat","event_system","");

s_ui.Show_Form(300,300);



