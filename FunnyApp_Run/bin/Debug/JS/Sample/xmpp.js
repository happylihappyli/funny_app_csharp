
function send_msg_click(data){
    s_net.Send_Msg(s_ui.text_read("send_msg"));
}


function event_connected(data){
    s_ui.text_set("txt1","Connected");
}


function event_chat(data){
    var obj=JSON.parse(data);
    s_ui.text_set("txt1",obj.message);
    sys.TTS(obj.message);
    s_ui.Notification(obj.from,obj.message);
}

function event_lgin(data){
    s_ui.msg(data);
}


function event_msg(data){
    s_ui.msg(data);
}


s_ui.button_init("b1","发送",10,100,200,30,"send_msg_click","");

s_ui.text_init("txt1","接收到信息",10,10,200,30);
s_ui.text_init("send_msg","",10,50,200,50);

s_net.xmpp_connect("robot6.funnyai.com","test@robot6.funnyai.com","test","event_lgin","event_msg");

s_ui.show_form(300,300);



