
[[[..\\data\\common_string.js]]]


function send_msg_click(data){
    var msg=s_ui.text_read("send_msg");
    s_net.xmpp_send("admin@robot6.funnyai.com",msg);
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
    s_net.xmpp_send("admin@robot6.funnyai.com","test");
    
    //s_ui.msg(data);
}


function event_msg(data){
    var obj=JSON.parse(data);
    
    s_ui.Form_Title("xmpp："+obj.from);

    var body=obj.body;
    body=body.replaceAll("\\n","\r\n");
    
    s_ui.text_set("txt1",body);
}


s_ui.textbox_init("txt1","接收到信息",10,10,200,100);
s_ui.text_init("send_msg","",10,150,200,50);

s_ui.button_init("b1","发送",10,200,200,30,"send_msg_click","");

s_net.xmpp_connect("robot6.funnyai.com","test","test","event_lgin","event_msg");

s_ui.show_form(300,300);
s_ui.Form_Title("xmpp");




