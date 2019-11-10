//这个功能在窗口写：hello word
var content="";
function screen_match(data){
    content=s_ui.screen_match("D:\\test.bmp");
    s_ui.text_set("txt",content); 
}

function show_text2(data){
    content+=data+"\r\n";
    s_ui.text_set("txt",content); 
}

s_ui.button_init("b_match","匹配",10,10,100,30,"screen_match","hello");
s_ui.textbox_init("txt","显示信息：",50,50,500,200);
s_ui.show_form(600,400);
s_ui.Form_Title("写字板");


