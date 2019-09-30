//API参考 https://github.com/leanote/leanote/wiki/leanote-api

var content="";
var email="";
var password="";
var user_id="";
var token="";

function post_click(data){
    email=s_ui.text_read("name");
    password=s_ui.text_read("password");
    var data2="email="+s_string.urlencode(email)
        +"&pwd="+s_string.urlencode(password);
    var a=s_net.http_post("http://www.funnyai.com:9000/api/auth/login",data2);
    var obj=JSON.parse(a);
    user_id=obj.UserId;
    token=obj.Token;
    s_ui.text_set("txt",a); 
}

function get_notebooks(data){
    
    email=s_ui.text_read("name");
    password=s_ui.text_read("password");
    var data2="email="+s_string.urlencode(email)
        +"&pwd="+s_string.urlencode(password);
        
    var a=s_net.http_post("http://www.funnyai.com:9000/api/notebook/getNotebooks?userId="+user_id+"&token="+token,data2);
    
    
    
    s_ui.text_set("txt",a); 
    
    init_tree(a);
    
}

function init_tree(data){
    
    
    s_string.json_array(data,"jarray");
    var count=s_string.json_array_length("jarray");
    
    //s_ui.msg(count+"");
    
    for (var i=0;i<count;i++){
        var ParentNotebookId=s_string.json_array_item("jarray",i,"ParentNotebookId");
        if (ParentNotebookId==""){
            var NotebookId=s_string.json_array_item("jarray",i,"NotebookId");
            var Title=s_string.json_array_item("jarray",i,"Title");
        
            s_ui.Tree_Add_Node_Root("tree1",NotebookId,Title,"Node_Click2");
        }
    }
    
}

function read_password(data){
    var strLine=s_file.read("D:/Net/Web/password_leanote.txt");
    var strPassword=s_string.decrypt_private_key("D:/Net/Web/id_rsa",strLine);
    s_ui.text_set("password",strPassword);
}

//保存密码
function save_password(data){
    var strPassword=s_ui.text_read("password");
    var strLine=s_string.encrypt_public_key("D:/Net/Web/public/id_rsa_happyli.pem.pub",strPassword);
    s_file.save("D:/Net/Web/password_leanote.txt",strLine);
}

function Node_Click2(data){
    //s_ui.msg(data);
}

s_ui.text_init("name","admin",10,10,100,30);
s_ui.password_init("password","",150,10,100,30);

s_ui.button_init("b_login","登录",250,10,100,30,"post_click","");
s_ui.button_init("b_save", "Save",350,10,100,30,"save_password","");

s_ui.button_init("b_test","Test",10,50,100,30,"get_notebooks","");

s_ui.Tree_Init("tree1",10,100,200,380);
    
s_ui.textbox_init("txt","显示信息：",250,100,300,380);
s_ui.Show_Form(800,600);
s_ui.Form_Title("蚂蚁笔记");

read_password("");
