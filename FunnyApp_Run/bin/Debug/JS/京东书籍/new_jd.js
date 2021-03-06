
var Path_Data ="";
var Path_Index="";
var Path_Seg="";
var auto=s_sys.value_read("auto");
var ISBN="";

[[[..\\data\\common_string.js]]]

function init(data){
    
    Path_Data =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","book","Path_Data");
    Path_Index=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","book","Path_Index");
    Path_Seg  =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","book","Path_Seg");
    
    if (Path_Data=="" || Path_Index=="" || Path_Seg==""){
        set_click("");
    }
}

function save_click(data){
    var ID=s_ui.text_read("txt1");
    s_sys.value_save("ID",ISBN);
    s_sys.value_save("Content",s_ui.text_read("txt_content"));
    s_ui.close();
}


function crawl_click(data){
    var page="";
    var file=disk+"view.txt";

    var url=s_ui.text_read("txt_url");
    if (url.startsWith("//")){
        url="https:"+url;
    }
    
    //*
    page=s_net.http_get(url,"gb2312");
    s_file.save_encode(file,page,"utf-8");
    
    var reg="";
    var array=new RegExp('>ISBN：(.*?)<','g').exec(page);
    if (array!=null && array.length>1) ISBN=array[1];
    
    array=new RegExp('<title>(.*?)</title>','g').exec(page);
    var title="";
    if (array!=null && array.length>1) title=array[1];
    
    reg='<a data-name="(.*?)" target="_blank" href="//book.jd.com/writer/';
    array=new RegExp(reg,'g').exec(page)
    var author;
    if (array!=null && array.length>1) author=array[1];
    
    reg='<img data-img="1" width=".*?" height=".*?" src="(.*?)"';
    array=new RegExp(reg,'g').exec(page);
    var img;
    if (array!=null && array.length>1) img=array[1];
    
    array=new RegExp('<li title="(.*?)" clstag=".*?chubanshe_3">','g').exec(page);
    var publisher;
    if (array!=null && array.length>1) publisher=array[1];
    
    
    if (title!=""){
        var content="{\"isbn\":\""+ISBN+"\",\r\n"+
                    "\"title\":\""+title+"\",\r\n"+
                    "\"author\":\""+author+"\",\r\n"+
                    "\"img\":\""+img+"\",\r\n"+
                    "\"publisher\":\""+publisher+"\",\r\n"+
                    "}\r\n";
        s_ui.text_set("txt_content",content);
    }else{
        s_ui.text_set("txt_content","");
    }
    
    if (auto=="1"){
        save_click("");
    }
}



s_ui.textbox_init("txt_content","",10,100,600,300);
s_ui.font_size("txt_content",18);
s_ui.control_dock("txt_content","fill");

s_ui.panel_init("panel1",0,0,500,30,"top");
s_ui.panel_init("panel2",0,0,500,30,"top");

s_ui.text_init("txt_url","",10,30,500,30);
s_ui.text_set("txt_url",s_sys.value_read("url"));


s_ui.button_init("b_save","保存",250,30,200,30,"save_click","");
s_ui.button_init("b_crawl","抓",250,30,100,30,"crawl_click","");


s_ui.font_size("txt_url",12);

s_ui.panel_add("panel1","b_save","left");

s_ui.panel_add("panel2","b_crawl","left");
s_ui.panel_add("panel2","txt_url","left");




init("");

if (auto=="1"){
    crawl_click("");
}else{
    s_ui.show_form(750,600);
    s_ui.Form_Title("抓取京东书籍");
}



