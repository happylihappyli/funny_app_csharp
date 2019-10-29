
var userName="none";
var log_msg="";

function save_click(data){

    var file=s_ui.text_read("txt_file");
    
    s_sys.value_save("cmd","ls -al "+file);
    
    s_ui.close();
}

var file=s_sys.value_read("file");

s_ui.label_init("lb_memo","ls -al 查看目录文件",50,30);

s_ui.label_init("lb_file","目录名:",10,100);
s_ui.text_init("txt_file","/root/happyli",150,100,300,30);

s_ui.button_init("b1_save","查看",150,250,100,30,"save_click","");


//其他属性
s_ui.button_default("b1_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("查看目录文件");
