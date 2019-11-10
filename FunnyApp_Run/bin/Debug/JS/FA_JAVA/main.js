
[[[..\\data\\common_string.js]]]


function run_click(){
    var line=s_ui.listbox_text("list1");
    var strSplit=line.split("|");
    var file=strSplit[0];
    var head1="java -jar E:\\happyli\\Jar\\funny_app\\funny_app.jar E:\\CloudStation\\Robot5\\GitHub\\funny_app_java\\funny_app\\js\\";
    var head2="java -jar E:\\happyli\\Jar\\funny_js\\funny_js.jar E:\\CloudStation\\Robot5\\GitHub\\funny_app_java\\funny_js\\js\\";
    var cmds;
    if (file.startsWith(":")){
        cmds=head2+file.substr(1);
    }else{
        cmds=head1+file;
    }
    
    s_ui.Run_Shell(cmds);
    if (strSplit.length>1){
        s_ui.Run_JS_Out(strSplit[1]);
    }
}

function fold_open(data){
    s_file.Open_Fold(s_sys.path_app()+"\\config\\funny_app.txt");
}

s_ui.button_init("b_run","运行",10,10,200,30,"run_click","");
s_ui.button_init("b_file","文件",250,10,200,30,"fold_open","");

s_ui.listbox_init("list1",10,50,550,500);

s_ui.listbox_from_file("list1",s_sys.path_app()+"\\config\\funny_app.txt");


s_ui.textbox_init("txt_error","错误信息：",600,50,300,500);


s_ui.show_form(960,650);

s_ui.Form_Title("FunnyApp.jar Test");





