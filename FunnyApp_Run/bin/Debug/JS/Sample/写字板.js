//这个功能在窗口写：hello word
var content="";
function show_text(data){
    content+=data+"\r\n";
    s_ui.text_set("txt",content); 
}

function show_text2(data){
    content+=data+"\r\n";
    s_ui.text_set("txt",content); 
}

s_ui.button_init("b_writer","写一段话",10,10,100,30,"show_text","hello");
s_ui.button_init("b_writer1","写3段论",110,10,100,30,"show_text2","你好\r\n你好\r\n你好\r\n");
s_ui.textbox_init("txt","显示信息：",50,50,500,200);
s_ui.Show_Form(600,400);
s_ui.Form_Title("写字板");


