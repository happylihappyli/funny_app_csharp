
[[[..\\data\\default.js]]]

function key_click(data){
    s_ui.msg("一会打开程序，请按两个回车，自动关闭窗口");
    s_ui.Run_App("C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe"," -m PEM -t rsa -b 2048 -C test -f "+disk+"/Net/Web/id_rsa");
    s_ui.text_set("txt1","把这个文件 "+disk+"\\Net\\Web\\id_rsa.pub 发到服务器，让管理员设置");
}

function set_click(data){
    //var path1=s_sys.UserProfile();
    var hosts=s_ui.combox_text("cb_hosts");
    //s_ui.text_set("txt1",path1);
    var content="{\n"+
    "  \"id_rsa\": \""+disk+"/Net/Web/id_rsa\",\n"+
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

    var path=s_sys.path_app();
    s_file.save(disk+"\\Net\\Web\\set.json",content);
}

function run_click(data){

    var path=s_sys.path_app();
    s_ui.text_set("txt1",disk+"\\Net\\Web\\main.exe -config "+disk+"\\Net\\Web\\set.json");
    s_ui.Run_App(disk+"\\Net\\Web\\main.exe","-config "+disk+"\\Net\\Web\\set.json");
    //s_ui.text_set("txt1",a);
    
}


function proxy1_click(data){
    s_net.Set_Proxy("127.0.0.1","1315");
}


function proxy2_click(data){
    s_net.Set_Proxy("127.0.0.1","1316");
}

function unset_proxy_click(data){
    s_net.UnSet_Proxy();
}



s_ui.textbox_init("txt1","先创建目录 \n "+disk+"\\Net\\Web\\ \n 然后点击下面按钮",10,10,500,90);
s_ui.combox_init("cb_hosts","144.202.65.8",10,110,200,30);
s_ui.combox_add("cb_hosts","144.202.65.8");


s_ui.button_init("b1","创建key",10,150,200,30,"key_click","0");

s_ui.button_init("b1_2","生成配置",250,150,200,30,"set_click","0");

s_ui.button_init("b2_1","运行",10,200,200,30,"run_click","0");

s_ui.button_init("b2_2","设置代理(smart)",10,250,200,30,"proxy1_click","0");
s_ui.button_init("b2_3","设置代理(normal)",250,250,200,30,"proxy2_click","0");

s_ui.button_init("b3_1","不用代理",10,300,200,30,"unset_proxy_click","");

s_ui.show_form(560,500);

s_ui.Form_Title("科学上网");

s_ui.Tray_Show(s_sys.path_js()+"\\proxy.ico");