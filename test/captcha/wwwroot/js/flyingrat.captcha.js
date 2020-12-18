(function (window, $) {
    function captchaSlider(method) {
        let func = {
            init: init,
            validated: validated,
            finished: finished,
            destory: destory,
            options: {
                sliderRoot: null,
                slider: null,
                background: null,
                image: null,
                sliderWidth:0
            }
        }
        func.options.sliderImg = method.container.querySelector(".captcha-slider-img");
        func.options.sliderRoot = method.container.querySelector(".captcha-control-slider");
        func.options.slider = func.options.sliderRoot.querySelector(".captcha-slider");
        func.options.sliderBackground = func.options.sliderRoot.querySelector(".captcha-slider-bg");
        func.options.sliderWidth = func.options.sliderRoot.clientWidth;
        let isMove = false,controlX=0;
        function init(){
            destory();
            let $slider = func.options.slider, gapImg = func.options.sliderImg, isMove = false;
            gapImg.src = method.data.isAction ? method.data.gap + "?tk=" + method.data.tk : method.data.gap;
            gapImg.style.top = getNumberPx(method.data.y);
            gapImg.style.display = "block";
            gapImg.onload = function () {
                func.options.sliderRoot.style.display = "block";
                $slider.style.width = getNumberPx(gapImg.clientWidth);
                func.options.sliderWidth = func.options.sliderRoot.clientWidth - $slider.clientWidth;
                $slider.addEventListener("mousedown", moveDown);
            };
        }
        function validated() {
            method.modal.show();
        }
        function finished() {
            method.modal.hidden();
        }
        function destory() {
            window.onmouseup = null;
            window.onmousemove = null;
            show(func.options.sliderRoot, true);
            show(func.options.sliderImg, true);
            if (func.options.slider.removeEventListener) {
                func.options.slider.removeEventListener("mousedown", moveDown);
            };
        }
        function moveDown(event) {
            let ev = event || window.event;
            controlX= ev.clientX;
            isMove = true;
            window.onmouseup = moveUp;
            window.onmousemove = move;
        };
        function move(event) {
            if (!isMove) return;
            let ev = event || window.event;
            ev.preventDefault();
            let px = ev.clientX;
            let mx = px - controlX;
            if (mx <= 0) mx = 0;
            if (mx > func.options.sliderWidth) mx = method.options.sliderWidth;
            if (func.options.slider) func.options.slider.style.left = getNumberPx(mx);
            if (func.options.sliderImg) func.options.sliderImg.style.left = getNumberPx(mx);
            if (func.options.sliderBackground) func.options.sliderBackground.style.width = getNumberPx(mx + func.options.slider.clientWidth);
        };
        function moveUp(event) {
            if (!isMove) return;
            isMove = false;
            sliderPoint(parseInt(func.options.sliderImg.style.left), parseInt(func.options.sliderImg.style.top));
            if (func.options.slider) func.options.slider.style.left = 0;
            if (func.options.sliderImg) func.options.sliderImg.style.left = 0;
            if (func.options.sliderBackground) func.options.sliderBackground.style.width = 0;
        };
        function sliderPoint(x, y) {
            let result =method.validatePoint(x, y);
            if (result.validate) method.addPoint(x, y);
            method.verify(result);
        };
        return func;
    }
    function captchaClick(method) {
        let func = {
            init: init,
            validated: validated,
            finished: finished,
            destory: destory,
            options: {
                pointIcon: [],
                css: {
                    point: "captcha-point captcha-points-"
                }
            }
        };
        function init() {
            destory();
            if (!method.data.tw) return;
            method.options.imgRoot.onclick = clickPoint;
            let div = document.createElement("div");
            let bg = method.options.isAction ? method.data.bgGap + "?tk=" + method.data.tk : method.data.bgGap;
            div.style.width = getNumberPx(method.data.tw);
            div.style.height = getNumberPx(method.data.th);
            div.style.marginLeft = getNumberPx(5);
            div.style.backgroundImage = "url(" + bg + ")";
            div.style.backgroundPositionY = getNumberPx(-method.data.height);
            method.updateTips(div);
        }
        function validated() {
            method.modal.show();
        }
        function finished() {
            method.modal.hidden();
            clearIcons();
        }
        function destory() {
            method.options.imgRoot.onclick = null;
            clearIcons();
        }
        function clickPoint(event) {
            let x = event.layerX, y = event.layerY;
            let result = method.validatePoint(x, y);
            if (result.validate) {
                let index = method.points.length + 1;
                let max = method.maxPoint;
                drawPoint(x, y, index, function (div, index) {
                    if (max == index - 1 && method.autoValidate) return;
                    div.style.top = getNumberPx(y - div.clientHeight);
                    div.style.left = getNumberPx(x - (div.clientWidth >> 1));
                    div.onclick = function (event) {
                        if (method.verifying && method.autoValidate) return;
                        method.clearPoints(index - 1);
                        clearIcons(index - 1);
                        event.stopPropagation();
                    }
                })
                method.addPoint(x, y);
            }
            method.verify(result);
        }
        function drawPoint(x, y, index, callback) {
            let div = createNumber(x, y, index);
            method.options.imgRoot.appendChild(div);
            func.options.pointIcon.push(div);
            if (isFunction(callback)) callback(div, index);
            return div;
        }
        function createNumber(x, y, number) {
            number = isNaN(number) ? 0 : parseInt(number);
            let div = document.createElement("div");
            div.className = func.options.css.point + number;
            return div;
        }
        function clearIcons(index) {
            index = isNaN(index) ? 0 : parseInt(index);
            let icons = func.options.pointIcon;
            if (icons && !icons.length) return;
            func.options.pointIcon = icons.slice(0, Math.max(index, 0));
            icons.slice(Math.min(index, icons.length)).every(function (item) {
                item.remove();
                return true;
            });
        }
        return func;
    }

    function captcha(container, options) {
        function template() {
            let func = {
                options: {
                    url: null,
                    points: [],
                    method: {
                        setRequestData: null,
                        allowRefresh: null,
                        validatePoint: null,
                        validate: null,
                        validated: null,
                        btn_refresh: null,
                        setData: null,
                        setResult: null,
                        ready: null
                    },
                    autoValidate: true,
                    autoRefresh: true,
                    divImg: null,
                    canvas: null,
                    controlRoot: null,
                    imgRoot: null,
                    tips: null,
                    modal: null
                },
                tools: {},
                container: null,
                isValidate: false,
                verifying: false
            };
            return func;
        }
        let method = new template();
        method.container = container || document.querySelector("#flyingrat");
        method.options.controlRoot = method.container.querySelector(".captcha-control");
        method.options.canvas = method.container.querySelector("canvas");
        method.options.divImg = method.container.querySelector(".captcha-img");
        method.options.imgRoot = method.container.querySelector(".captcha-control-image");
        method.options.tips = method.container.querySelector(".captcha-control-tips");
        method.options.modal = method.container.querySelector(".captcha-modal");
        $.extend(method.options, options);
        
        let refbtn = method.container.querySelector(".captcha-control-refresh");
        if (refbtn) {
            refbtn.onclick = function () {
                if (isFunction(method.options.method.btn_refresh)) method.options.method.btn_refresh();
                else autoDrawImage();
            };
            addTool("refresh", refbtn);
        }

        let captchas = (function () {
            let func = {
                "slidercaptcha": { bindCaptcha: captchaSlider(method), type: 2, ignoreAutoValidate: true},//sliderCaptcha
                "pointcaptcha": { bindCaptcha: captchaClick(method), type: 3, ignoreAutoValidate: false}//pointCaptcha}
            };
            return func;
        })();
        let captchaData = mapData();

        defineProperty();
        function defineProperty() {
            Object.defineProperties(method, {
                "name": { get: function () { return method.data.name.toLowerCase() || null; } },
                "type": { get: function () { return method.current.type || null } },
                "data": { get: function () { return captchaData } },
                "captchas": { get: function () { return captchas; } },
                "current": { get: function () { return captchas[method.name];} },
                "points": { get: function () { return method.options.points; } },
                "clearPoints": { get: function () { return clearPoints; } },
                "maxPoint": { get: function () { return method.data.x || 0; } },
                "addCaptcha": { get: function () { return addCaptcha; } },
                "updateOptions": { get: function () { return updateOptions; } },
                "mapData": { get: function () { return mapData; } },
                "mapResult": { get: function () { return mapResult; } },
                "drawCaptcha": { get: function () { return drawImage; } },
                "drawPoint": { get: function () { return drawPoint;}},
                "destroy": { get: function () { return destroy; } },
                "refresh": { get: function () { return autoDrawImage; } },
                "validate": { get: function () { return requestVerfiy; } },
                "validated": { get: function () { return verified; } },
                "validatePoint": { get: function () { return validatePoint; } },
                "addPoint": { get: function () { return addPoint; } },
                "verify": { get: function () { return verify; } },
                "autoValidate": { get: function () { return isAutoType(); } },
                "updateTips": { get: function () { return updateTips; } },
                "modal": { get: function () { return { show:showModal,hidden:hiddenModal } } }
            })
        }

        function initCaptcha() {
            clearPoints();
            destroyCaptcha();
            captchaData = null;
            method.isValidate = false;
            method.verifying = false;
            hiddenModal();
        }
        function destroy() {
            destroyCaptcha();
            clearPoints();
            $.extend(method, new template());
            method = null;
        }

        function addTool(key, tool) {
            if (!key) return false;
            method.tools[key] = tool;
        }

        function autoDrawImage() {
            $.get(method.options.url, null, function (data) {
                drawImage(data);
            })
        }
        function drawImage(data) {
            if (!data) { autoDrawImage();return;}
            let canvas = method.options.canvas;
            canvas == null ? drawDivImage(data) : drawCanvasImage(data);
        }
        function drawDivImage(data) {
            initCaptcha();
            if (isFunction(method.options.method.setData)) data = method.options.method.setData(data);
            data = mapData(data);
            captchaData = data;
            if (!data.bgGap) return readyCaptcha();
            show(container, false);
            let div = method.options.divImg;
            if (div == null) return;
            let url = data.isAction ? data.bgGap + "?tk=" + data.tk : data.bgGap;
            let image = document.createElement("div");
            div.style.width = getNumberPx(data.width);
            div.style.height = getNumberPx(data.height);
            div.innerHTML = "";
            let change = JSON.parse(data.change);
            let dataIndex = JSON.parse(data.index);
            let width = Math.max(Math.floor(data.width / data.col), 1);
            let height = Math.max(Math.floor(data.height / data.row), 1);
            let images = [];
            let xxpos = 0;
            for (let i = 0; i < dataIndex.length; i++) {
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
                item.style.backgroundPositionX = getNumberPx(-xxpos);
                item.style.backgroundPositionY = getNumberPx(-y * height);
                item.style.width = getNumberPx(curWidth);
                item.style.height = getNumberPx(height);
                item.style.float = "left";
                xxpos += curWidth;
                images[index] = item;
            }
            for (let i = 0; i < images.length; i++) {
                div.appendChild(images[i]);
            }
            readyCaptcha();
        }
        function drawCanvasImage(data) {
            initCaptcha();
            if (isFunction(method.options.method.setData)) data = method.options.method.setData(data);
            data = mapData(data);
            captchaData = data;
            if (!data.bgGap) return readyCaptcha();
            show(container, false);
            let canvas = method.options.canvas;
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
                readyCaptcha();
            }
        }

        function addCaptcha(key, options) {
            if (!options || typeof options !="object" || !key || !options.init&&!isFunction(options.init) ) return false;
            let type = isNaN(options.type) ? 0 : parseInt(options.type);
            let captcha = { bindCaptcha: options.init(method), type: type, ignoreAutoValidate:false };
            method.captchas[key.toLowerCase()] = captcha;
            return captcha;
        }
        function updateOptions(key, options) {
            let captcha = method.captchas[key.toLowerCase()];
            if (!captcha) return false;
            $.extend(captcha.options, options);
            return captcha;
        }
        function destroyCaptcha(){
            Object.keys(captchas).forEach(function (key) {
                captchas[key].bindCaptcha.destory();
            })
        }
        function readyCaptcha() {
            initBindCaptcha();
            executeReady(method);
        }
        function initBindCaptcha() {
            let span = document.createElement("span");
            span.innerText = method.data.tips;
            updateTips(span, true);
            let captcha = method.captchas[method.name];
            if (captcha) excuteFunc(captcha.bindCaptcha.init);
        }
        function executeReady(captcha) {
            if (captcha && isFunction(captcha.options.method.ready)) captcha.options.method.ready(captcha);
        }
        function updateTips(el, isNew) {
            if (!method.options.tips) return;
            if (isNew) method.options.tips.innerHTML = "";
            method.options.tips.appendChild(el)
        }

        function validatePoint(x, y) {
            let custom = { custom: false, validate: false };
            if (method.verifying && method.autoValidate) return custom;
            if (isFunction(method.options.method.validatePoint)) {
                let result = method.options.method.validatePoint(x, y,method);
                custom.custom =true;
                custom.validate = !!result;
            } else {
                custom.validate = method.points.length < method.maxPoint;
            }
            return custom;
        }
        function addPoint(x, y) {
            let point = { x: x, y: y };
            method.options.points.push(point);
            return point;
        }
        

        function clearPoints(index) {
            index = isNaN(index) ? 0 : parseInt(index);
            if (!method) return;
            let points = method.options.points;
            method.options.points = points.slice(0, Math.max(index, 0));
        }
        function delayedClearPoints(second, callback) {
            delayedFuc(second, clearPoints, callback);
        }

        function verify(custom) {
            if (custom.custom || method.isValidate || method.maxPoint != method.points.length) return;
            method.verifying = true;
            if (isFunction(method.options.method.validate)) {
                let isvalid = method.options.method.validate(verified,method);
                if (isvalid) return;
            }
            if (method.autoValidate) requestVerfiy();
            if (method.current.type != 3) requestVerfiy();
        }
        function requestVerfiy() {
            let data = method.data, points = method.points;
            if (method.maxPoint != points.length) return;
            if (!method.verifying) method.verifying = true;
            let redata = { points: points, tk: data.tk };
            if (isFunction(method.options.method.setRequestData)) redata = method.options.method.setRequestData(points, data);
            $.post(data.validate, redata, function (data) {
                verified(data);
            })
                .fail(function () {
                    method.verifying = false;
                });
        }
        function verified(data) {
            if (!method) return;
            let udata = null;
            if (isFunction(method.options.method.setResult)) udata = method.options.method.setResult(data);
            data =mapResult( udata || data);
            method.isValidate = data.succeed;
            excuteFunc(method.current.bindCaptcha.validated)
            if (data.refresh && (method.options.autoRefresh || isFunction(method.options.allowRefresh) && method.options.allowRefresh(data, method))) {
                delayedFuc(1, autoDrawImage);
                return;
            }
            if (isFunction(method.options.method.validated)) method.options.method.validated(data);
            delayedClearPoints(1, function () { if (!method) return; method.verifying = false; excuteFunc(method.current.bindCaptcha.finished)});
        }

        function mapResult(data) {
            let innerData = { token: null, succeed: false, refresh: false };
            $.extend(innerData, data);
            return innerData;
        }
        function mapData(data) {
            let innerData = { index: null, change: null, width: 0, height: 0, col: 0, row: 0, x: 0, y: 0, tk: null, bgGap: null, gap: null, full: null, validate: null, isAction: false, tips: null, tw: 0, th: 0, type: 0,name:null};
            $.extend(innerData, data);
            return innerData;
        }

        function showModal() {
            show(method.options.modal, false);
        }
        function hiddenModal() {
            show(method.options.modal, true);
        }
        function isAutoType() {
            return !method.current.ignoreAutoValidate && method.options.autoValidate;
        }
        return method;
    }

    function delayedFuc(second, func, callback) {
        setTimeout(function () {
            if (isFunction(func)) func();
            if (isFunction(callback)) callback();
        }, parseInt(second) * 1000)
    }
    function show(element, hidden) {
        if (!element) return;
        element.style.display = !!hidden ? "none" : "block";
    }

    function getNumberPx(number) {
        return parseInt(number) + "px";
    }

    function excuteFunc(func) {
        if (isFunction(func)) func();
    }
    function isFunction(func) {
        if (func && typeof func === "function") return true;
        return false;
    }
    window.captcha = captcha;
})(window, jQuery)
