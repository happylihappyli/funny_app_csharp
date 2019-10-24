
var disk="D:";
var userName="none";
var md5="";


var css_head='<html><head>\n'
+'<link href="http://www.funnyai.com/Common/css/default.css" type="text/css" rel="stylesheet" />\n'
+'<link href="http://www.funnyai.com/Common/css/table.css" type="text/css" rel="stylesheet" />\n'
+'<body>\n';

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}


function sys_read_ini(){
    var path=s_sys.Path_App();
    //var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    //s_ui.msg(disk);
    var userName2=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    //s_ui.msg(userName2);
    
    md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    //s_ui.msg(userName2);
    return userName2;
}


function sys_get_token(){
    var url="http://www.funnyai.com/login_get_token_json.php";
    var name=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    var md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    var result=s_net.http_post(url,data);
    var token="";
    if (result.indexOf("登录成功")>-1){
        var strSplit=result.split("=");
        token=strSplit[2];
    }
    return token;
}