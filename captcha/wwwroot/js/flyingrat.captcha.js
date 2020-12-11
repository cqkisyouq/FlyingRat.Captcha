(function (window,$) {
    window.captcha = function (container, options) {
        let method = {
            drawCaptcha: drawImage,
            options: {
                url: null,
                points:[],
                validate: null,
                validateCallback: null,
                x: 0,
                y: 0,
                isMove: false,
                divImg: null,
                canvas: null,
                slider: null,
                sliderBackground:null,
                sliderImg: null,
                sliderWidth: 0,
                controlRoot: null,
                sliderRoot:null,
                imgRoot: null,
                tips:null,
                data: null
            },
            container: null,
            isValidate: false
        }
        method.container = container || method.container;
        method.options.sliderRoot = method.container.querySelector(".captcha-control-slider");
        method.options.slider = method.options.sliderRoot.querySelector(".captcha-slider");
        method.options.sliderBackground = method.options.sliderRoot.querySelector(".captcha-slider-bg");
        method.options.sliderImg = method.container.querySelector(".captcha-slider-img");
        method.options.controlRoot = method.container.querySelector(".captcha-control");
        method.options.canvas = method.container.querySelector("canvas");
        method.options.divImg = method.container.querySelector(".captcha-img");
        method.options.imgRoot = method.container.querySelector(".captcha-control-image");
        method.options.tips = method.container.querySelector(".captcha-control-tips");
        method.options.sliderWidth = method.options.sliderRoot.clientWidth;
        $.extend(method.options, options);

        function autoDrawImage() {
            $.get(method.options.url,null, function (data) {
                drawImage(data);
            })
        }
        function drawImage(data) {
            if (!data) {
                autoDrawImage();
                return;
            }
            let canvas = method.options.canvas, container = method.container;
            method.options.data = data;
            method.container.style.width = data.width;
            method.isValidate = false;
            method.options.points = [];
            if (canvas == null) {
                drawDivImage(data);
                return;
            }
            container.style.display = "block";
            let ctx = canvas.getContext("2d");
            canvas.setAttribute("width", data.width);
            canvas.setAttribute("height", data.height);
            let image = new Image();
            image.src = data.isAction ? data.bgGap + "?tk=" + data.tk : data.bgGap;
            image.onload = function () {
                let dataIndex = JSON.parse(data.index);
                let change = JSON.parse(data.change);
                let width = Math.max(Math.floor(data.width / data.col), 1);
                let height = Math.max(Math.floor(data.height / data.row), 1);
                let xxpos = 0;
                for (let i = 0; i < dataIndex.length; i++) {
                    let index = dataIndex[i];
                    let y = Math.floor(index / data.col);
                    let x = index % data.col;

                    let xy = Math.floor(i / data.col);
                    let xx = i % data.col;
                    let curWidth = width;
                    let xpos = x * width;
                    if (xx == 0) xxpos = 0;
                    if (change && change.indexOf(index) >= 0) {
                        curWidth = Math.max(data.width - (data.col - 1) * width, 0);
                        curWidth = Math.max(curWidth, 0);
                    }
                    ctx.drawImage(image, xxpos, xy * height, curWidth, height, xpos, y * height, curWidth, height);
                    xxpos += curWidth;
                }
                bindControl();
            }
        };
        function drawDivImage(data) {
            let div = method.options.divImg, container = method.container;
            method.options.data = data;
            if (div == null) return;
            container.style.display = "block";
            let url =data.isAction?data.bgGap + "?tk=" + data.tk:data.bgGap;
            let image = document.createElement("div");
            div.style.width = data.width;
            div.style.height = data.height;
            //div.style.backgroundImage = "url(" + url + ")";
            div.innerHTML = "";
            let change = JSON.parse(data.change);
            let dataIndex = JSON.parse(data.index);
            let width = Math.max(Math.floor(data.width / data.col), 1);
            let height = Math.max(Math.floor(data.height / data.row), 1);
            let images = [];
            let xxpos = 0;
            for (var i = 0; i < dataIndex.length; i++) {
                let index = dataIndex[i];
                let y = Math.floor(i / data.col);
                let x = i % data.col;
                let curWidth = width;
                if (change && change.indexOf(index) >= 0) {
                    curWidth = Math.max(data.width - (data.col - 1) * width, 0);
                    curWidth = Math.max(curWidth, 0);
                }
                if (x == 0) xxpos = 0;
                let item = image.cloneNode();
                item.style.backgroundImage = "url(" + url + ")";
                item.style.backgroundPositionX = -xxpos + "px";
                item.style.backgroundPositionY = -y * height + "px";
                item.style.width = curWidth + "px";
                item.style.height = height + "px";
                item.style.float = "left";
                xxpos += curWidth;
                images[index] = item;
            }
            for (var i = 0; i < images.length; i++) {
                div.appendChild(images[i]);
            }
            bindControl();
        };
        function moveDown(event) {
            let ev = event || window.event;
            method.options.x = ev.clientX;
            method.options.isMove = true;
            window.onmouseup = moveUp;
            window.onmousemove = move;
        };
        function move(event) {
            if (!method.options.isMove) return;
            let ev = event || window.event;
            ev.preventDefault();
            let px = ev.clientX;
            let mx = px - method.options.x;
            if (mx <= 0) mx = 0;
            if (mx > method.options.sliderWidth) mx = method.options.sliderWidth;
            if (method.options.slider) method.options.slider.style.left = mx;
            if (method.options.sliderImg) method.options.sliderImg.style.left = mx;
            if (method.options.sliderBackground) method.options.sliderBackground.style.width = mx;
        };
        function moveUp(event) {
            if (!method.options.isMove) return;
            sliderPoint(parseInt(method.options.sliderImg.style.left),parseInt(method.options.sliderImg.style.top));
            method.options.isMove = false;
            if (method.options.slider) method.options.slider.style.left = 0;
            if (method.options.sliderImg) method.options.sliderImg.style.left = 0;
            if (method.options.sliderBackground) method.options.sliderBackground.style.width = 0;
        };

        function bindControl() {
            var span = document.createElement("span");
            span.innerText = method.options.data.tips;
            updateTips(span, true); 
            switch (method.options.data.type) {
                case 2:
                    bindControlSlider();
                    break;
                case 3:
                    bindControlClick();
                    break;
                default:
                    break;
            }
        }
        function bindControlSlider() {
            let $slider = method.options.slider, gapImg = method.options.sliderImg;
            gapImg.src =method.options.data.isAction?method.options.data.gap + "?tk=" + method.options.data.tk:method.options.data.gap;
            gapImg.style.top = method.options.data.y;
            gapImg.style.display = "block";
            method.options.sliderRoot.style.display = "block";
            method.options.sliderWidth = method.options.sliderRoot.clientWidth - $slider.clientWidth;
            if ($slider.removeEventListener) {
                $slider.removeEventListener("mousedown", moveDown);
            }
            $slider.addEventListener("mousedown", moveDown);
        };
        function bindControlClick() {
            method.options.sliderRoot.style.display = "none";
            method.options.sliderImg.style.display = "none";
            method.options.imgRoot.onclick = clickPoint;
            if (!method.options.data.tw) return;

            var div =document.createElement("div");
            var bg = method.options.isAction ? method.options.data.bgGap + "?tk=" + method.options.data.tk : method.options.data.bgGap;
            div.style.width = method.options.data.tw;
            div.style.height = method.options.data.th;
            div.style.marginLeft = 5;
            div.style.backgroundImage = "url(" + bg + ")";
            div.style.backgroundPositionY = -method.options.data.height +"px";
            updateTips(div);
        }

        function updateTips(el,isNew)
        {
            if (isNew) method.options.tips.innerHTML = "";
            method.options.tips.appendChild(el)
        }

        function sliderPoint(x, y) {
            validatePoint(x, y);
        }
        function clickPoint(event) {
            validatePoint(event.layerX, event.layerY);
        }

        function validatePoint(x, y) {
            if (method.options.validate && typeof method.options.validate === "function") {
                method.options.validate(x, y);
            }
            let data = method.options.data, points = method.options.points;
            if (points.length < data.x) {
                points.push({ x: x, y: y });
            }
            if (points.length == data.x) {
                $.post(data.validate, { points: points, tk: data.tk, type: data.name }, function (data) {
                    method.isValidate = data.succeed;
                    if (method.options.validateCallback && typeof method.options.validateCallback === "function") method.options.validateCallback(data);
                });
                method.options.points = [];
            }
        }
        return method;
    };
})(window,jQuery)
