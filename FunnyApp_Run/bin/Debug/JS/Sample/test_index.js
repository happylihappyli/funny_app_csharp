

function index_create(data){
    s_index.Create_Start(disk+"\\Net\\Web\\index",true);
    s_index.Add_Document("1","你好中国");
    s_index.Add_Document("2","中国社会发展");
    s_index.Create_End();
}



function index_add(data){
    s_index.Create_Start(disk+"\\Net\\Web\\index",false);
    s_index.Add_Document("3","发展中国家");
    s_index.Create_End();
}



function search(data){
    var seg=s_index.Seg(s_ui.text_read("txt1"));
    var result=s_index.Search(disk+"\\Net\\Web\\index",seg);
    
    s_xml.init(result,"doc1");
    
    var count=s_xml.count("doc1","/data/item");
    
    var html="";
    for (var i=1;i<=count;i++){
        var id=s_xml.read("doc1","/data/item[position()="+i+"]/id");
        var content=s_xml.read("doc1","/data/item[position()="+i+"]/content");
        html+="id="+id+"<br>"+content+"<hr>";
    }
    s_ui.Web_Content("web",html);
}

function callback_init(data){
    s_ui.text_set("txt1","中国");
    s_ui.button_enable("b1",1);
    s_ui.button_enable("b2",1);
}

s_index.Init_Seg(disk+"Funny\\FunnyAI\\Data\\Dic\\Segmentation","callback_init");
s_ui.text_init("txt1","输入信息",10,10,200,30);
s_ui.button_init("b_search","搜索",250,10,200,30,"search","");

s_ui.button_default("b_search");

s_ui.button_init("b1","清空，创建文档",10,50,200,30,"index_create","");
s_ui.button_enable("b1",0);
s_ui.button_init("b2","添加文档",250,50,200,30,"index_add","");
s_ui.button_enable("b2",0);

s_ui.Web_Init("web",10,100,700,500);

s_ui.show_form(800,680);
s_ui.Form_Title("全文索引");

