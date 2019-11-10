
var userName="none";
var log_msg="";

function ok_click(data){

    var code=s_ui.text_read("txt_code");
    s_sys.value_save("id",code);
    s_ui.close();
}

var file=s_sys.value_read("file");

s_ui.label_init("lb_memo","申通快递单号查询",150,30);

s_ui.label_init("lb_file","单号:",10,100);
s_ui.text_init("txt_code","773012398601931",150,100,300,30);

s_ui.button_init("btn_ok","查询",150,250,100,30,"ok_click","");

//其他属性
s_ui.button_default("btn_ok");
s_ui.show_form(560,380);
s_ui.Form_Title("申通快递单号查询");
