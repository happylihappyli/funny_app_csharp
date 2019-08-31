
function send_msg_click(data){
    sys.Send_Msg(sys.Get_Text("send_msg"));
}


function event_connected(data){
    sys.Show_Text("txt1","Connected");
}


function event_chat(data){
    var obj=JSON.parse(data);
    sys.Show_Text("txt1",obj.message);
    sys.TTS(obj.message);
    sys.Notification(obj.from,obj.message);
}

function event_system(data){
    
}


sys.Button_Init("b1","发送",10,100,200,30,"send_msg_click","");

sys.Text_Init("txt1","接收到信息",10,10,200,30);
sys.Text_Init("send_msg","",10,50,200,50);

sys.Socket_Init("http://robot6.funnyai.com:8000","event_connected","event_chat","event_system","");

sys.Show_Form(300,300);



