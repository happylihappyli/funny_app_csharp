function key_click(data){
    sys.Msg("一会打开程序，请按两个回车，自动关闭窗口");
    sys.Run_App("C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe"," -m PEM -t rsa -b 2048 -C test -f D:/Net/Web/id_rsa");
    sys.Show_Text("txt1","把这个文件 D:\\Net\\Web\\id_rsa.pub 发到服务器，让管理员设置");
}

function set_click(data){
    //var path1=sys.UserProfile();
    var hosts=sys.Combox_Text("cb_hosts");
    //sys.Show_Text("txt1",path1);
    var content="{\n"+
    "  \"id_rsa\": \"D:/Net/Web/id_rsa\",\n"+
    "  \"local_smart\": \":1315\",\n"+
    "  \"local_normal\": \":1316\",\n"+
    "  \"remote\": \"ssh://root@"+hosts+":22\",\n"+
    "  \"blocked\": [\n"+
    "    \"angularjs.org\",\n"+
    "    \"golang.org\",\n"+
    "    \"google.com\",\n"+
    "    \"google.co.jp\",\n"+
    "    \"googleapis.com\",\n"+
    "    \"googleusercontent.com\",\n"+
    "    \"google-analytics.com\",\n"+
    "    \"gstatic.com\",\n"+
    "    \"twitter.com\",\n"+
    "    \"youtube.com\"\n"+
    "  ]\n"+
    "}";

    var path=sys.AppPath();
    sys.File_Save("D:\\Net\\Web\\set.json",content);
}

function run_click(data){

    var path=sys.AppPath();
    sys.Show_Text("txt1","D:\\Net\\Web\\main.exe -config D:\\Net\\Web\\set.json");
    sys.Run_App("D:\\Net\\Web\\main.exe","-config D:\\Net\\Web\\set.json");
    //sys.Show_Text("txt1",a);
    
}


function proxy1_click(data){
    sys.Set_Proxy("127.0.0.1","1315");
}


function proxy2_click(data){
    sys.Set_Proxy("127.0.0.1","1316");
}

function unset_proxy_click(data){
    sys.UnSet_Proxy();
}



sys.Add_Text_Multi("txt1","先创建目录 \n D:\\Net\\Web\\ \n 然后点击下面按钮",10,10,600,90);
sys.Add_Combox("cb_hosts","149.248.37.214",10,110,200,30);
sys.Combox_Add("cb_hosts","149.248.37.214");
sys.Combox_Add("cb_hosts","45.63.52.243");


sys.Add_Button("b1","创建key",10,150,200,30,"key_click","0");

sys.Add_Button("b1_2","生成配置",250,150,200,30,"set_click","0");

sys.Add_Button("b2_1","运行",10,200,200,30,"run_click","0");

sys.Add_Button("b2_2","设置代理(smart)",10,250,200,30,"proxy1_click","0");
sys.Add_Button("b2_3","设置代理(normal)",250,250,200,30,"proxy2_click","0");

sys.Add_Button("b3_1","不用代理",10,300,200,30,"unset_proxy_click","");

sys.Show_Form(700,500);


