
var userName="none";
var log_msg="";

function save_click(data){

    var name=s_ui.text_read("txt_name");
    var python_version=s_ui.combox_text("combox_python");
    
    s_sys.value_save("cmd","conda create --name "+name+" python="+python_version);
    
    s_ui.close();
}

var file=s_sys.value_read("file");

s_ui.label_init("lb_memo","activate xxx 进入该配置",50,30);

s_ui.label_init("lb_file","Name:",10,100);
s_ui.text_init("txt_name","test",150,100,300,30);

s_ui.label_init("lb_python","python:",10,150);
s_ui.combox_init("combox_python","3.7",150,150,300,30);
s_ui.combox_add("combox_python","3.7");
s_ui.combox_add("combox_python","3.6");
s_ui.combox_add("combox_python","3.5");



s_ui.button_init("b1_save","查看",150,250,100,30,"save_click","");


//其他属性
s_ui.button_default("b1_save");
s_ui.show_form(560,380);
s_ui.Form_Title("查看目录文件");
