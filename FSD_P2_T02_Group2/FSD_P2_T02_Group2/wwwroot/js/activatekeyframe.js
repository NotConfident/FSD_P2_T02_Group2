var style = document.documentElement.appendChild(document.createElement("style"));

move_rule = " move {\
        0% {\
            opacity: 0;\
            visibility: hidden;\
            -webkit-transform: translateY(-40px);\
            transform: translateY(-40px);\
        }\
        100% {\
            opacity: 1;\
            visibility: visible;\
            -webkit-transform: ranslateY(0);\
            transform: translateY(0);\
        }\
    }";

move_rule2 = " move {\
        0% {\
            opacity: 1;\
            visibility: visible;\
            -webkit-transform: ranslateY(0);\
            transform: translateY(0);\
        }\
        100% {\
            opacity: 1;\
            visibility: visible;\
            -webkit-transform: ranslateY(0);\
            transform: translateY(0);\
        }\
    }";

left_rule = " left {\
            0% {\
                opacity: 0;\
                width: 0;\
            }\
            100% {\
                opacity: 1;\
                padding: 20px 40px;\
                width: 500px;\
            }\
        }";

left_rule2 = " left {\
            0% {\
                opacity: 1;\
                padding: 20px 40px;\
                width: 500px;\
            }\
            100% {\
                opacity: 1;\
                padding: 20px 40px;\
                width: 500px;\
            }\
        }";

var action = $("#recordFailLogin").data("value");

if (CSSRule.KEYFRAMES_RULE) {
    if (action == "") {
        style.sheet.insertRule("@keyframes" + move_rule, 0);
        style.sheet.insertRule("@keyframes" + left_rule, 0);
    } else {
        style.sheet.insertRule("@keyframes" + move_rule2, 0);
        style.sheet.insertRule("@keyframes" + left_rule2, 0);
    }

} else if (CSSRule.WEBKIT_KEYFRAMES_RULE) {
    if (action == "") {
        style.sheet.insertRule("@-webkit-keyframes" + move_rule, 0);
        style.sheet.insertRule("@-webkit-keyframes" + left_rule, 0);
    } else {
        style.sheet.insertRule("@-webkit-keyframes" + move_rule2, 0);
        style.sheet.insertRule("@-webkit-keyframes" + left_rule2, 0);
    }

}