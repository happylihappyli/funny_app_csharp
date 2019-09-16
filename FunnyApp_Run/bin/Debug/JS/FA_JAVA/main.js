
function run_click(){
    var line=s_ui.ListBox_Text("list1");
    var strSplit=line.split("|");
    var head="java -jar E:\\happyli\\Jar\\funny_app\\funny_app.jar E:\\CloudStation\\Robot5\\GitHub\\funny_app_java\\funny_app\\js\\";
    var cmds=head+strSplit[0];
    //s_ui.Msg(cmds);
    s_ui.Run_Shell(cmds);
    if (strSplit.length>1){
        s_ui.Run_JS_Out(strSplit[1]);
    }
}

function fold_open(data){
    sys.Open_Fold(sys.App_Path()+"\\config\\funny_app.txt");
}

s_ui.Button_Init("b_run","运行",100,10,200,30,"run_click","");
s_ui.Button_Init("b_file","文件",300,10,200,30,"fold_open","");

s_ui.ListBox_Init("list1",100,50,500,200);

s_ui.ListBox_From_File("list1",sys.App_Path()+"\\config\\funny_app.txt");


s_ui.TextBox_Init("txt_error","错误信息：",100,300,500,200);


s_ui.Show_Form(800,600);

s_ui.Form_Title("FunnyApp.jar Test");





