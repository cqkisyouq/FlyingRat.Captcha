(function (window, $) {
    function captchaSlider(method) {
        let func = {
            before: before,
            init: init,
            validate: validate,
            validated: validated,
            finished: finished,
            exit: destory,
            allowVerify: canValidate,
            jsonData: getRequestData,
            hasShow:true,
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
        let isMove = false, controlX = 0;
        function before() {
            destory();
        }
        function init() {
            method.modal.show();
            method.showIcon(method.options.icon.refresh, false);
            let slider = func.options.slider, gapImg = func.options.sliderImg;
            controlX = 0; isMove = false;
            gapImg.src = method.data.isAction ? method.data.gap + "?tk=" + method.data.tk : method.data.gap;
            gapImg.style.top = getNumberPx(method.data.y);
            gapImg.style.display = "block";
            gapImg.onload = function () {
                func.options.sliderRoot.style.display = "block";
                slider.style.width = getNumberPx(gapImg.clientWidth);
                func.options.sliderWidth = func.options.sliderRoot.clientWidth - slider.clientWidth;
                slider.addEventListener("mousedown", moveDown);
                delayedFuc(0.5, function () {
                    method.showIcon(method.options.icon.refresh, true);
                    excuteFunc(method.modal.hidden);
                });
            };
        }
        function validate() {
            method.modal.show();
        }
        function validated() {
            if (method.succeed) return method.showIcon(method.options.icon.succeed, false);
            method.showIcon(method.options.icon.failed, false);
        }
        function finished() {
            method.modal.hidden();
            method.icon.hidden();
            initSlider();
        }
        function destory() {
            window.onmouseup = null;
            window.onmousemove = null;
            show(func.options.sliderRoot, true);
            show(func.options.sliderImg, true);
            if (func.options.slider.removeEventListener) {
                func.options.slider.removeEventListener("mousedown", moveDown);
            };
            initSlider();
        }
        function getRequestData() {
            if (method.maxPoint == method.points.length) {
                return method.createJsonData(true,{ points: method.points,tk:method.data.tk });
            }
            return method.createJsonData();
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
        };
        function sliderPoint(x, y) {
            if (!x) return;
            if (method.validatePoint(x, y)) method.addPoint(x, y);
            if (canValidate) method.validate();
        }
        function canValidate() {
            return method.maxPoint == method.points.length;
        }
        function initSlider() {
            if (func.options.slider) func.options.slider.style.left = 0;
            if (func.options.sliderImg) func.options.sliderImg.style.left = 0;
            if (func.options.sliderBackground) func.options.sliderBackground.style.width = 0;
        }
        return func;
    }
    function captchaClick(method) {
        let func = {
            before: before,
            init: init,
            validate: validate,
            validated: validated,
            finished: finished,
            exit: destory,
            jsonData: getRequestData,
            allowVerify: canValidate,
            hasShow: true,
            submit:null,
            options: {
                pointIcon: [],
                callback: {
                    submit: null
                },
                css: {
                    point: "captcha-point captcha-points-",
                    submit:"captcha-manual-submit"
                }
            }
        };
        function before() {
            destory();
        }
        function init() {
            if (!method.data.tw) return;
            method.modal.show();
            method.showIcon(method.options.icon.refresh,false);
            bindClick();
            let div = document.createElement("div");
            let bg = method.options.isAction ? method.data.bgGap + "?tk=" + method.data.tk : method.data.bgGap;
            div.style.width = getNumberPx(method.data.tw);
            div.style.height = getNumberPx(method.data.th);
            div.style.marginLeft = getNumberPx(5);
            div.style.backgroundImage = "url(" + bg + ")";
            div.style.backgroundPositionY = getNumberPx(-method.data.height);
            method.updateTips(div);
            delayedFuc(0.5, function () {
                method.showIcon(method.options.icon.refresh, true);
                excuteFunc(method.modal.hidden);
            });
        }
        function validate() {
            method.modal.show();
        }
        function validated() {
            if (method.succeed) return method.showIcon(method.options.icon.succeed, false);
            method.showIcon(method.options.icon.failed, false);
        }
        function finished() {
            method.modal.hidden();
            method.icon.hidden();
            clearIcons();
        }
        function destory() {
            method.options.imgRoot.onclick = null;
            if (!method.autoValidate && !!func.submit) show(func.submit, true);
            clearIcons();
            func.submit = null;
        }
        function getRequestData() {
            if (method.maxPoint == method.points.length) {
                return method.createJsonData(true, { points: method.points, tk: method.data.tk });
            }
            return method.createJsonData();
        }

        function clickPoint(event) {
            let x = event.layerX, y = event.layerY;
            if (method.validatePoint(x, y)) {
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
            if (canValidate) submitData();
        }
        function submitData(result) {
            method.validate(result);
        }
        function canValidate() {
            return method.points.length == method.maxPoint;
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
        function bindClick() {
            method.options.imgRoot.onclick = clickPoint;
            func.submit = method.container.querySelector("." + func.options.css.submit);
            if (!func.submit) return;
            if (!method.autoValidate) show(func.submit, false);
            func.submit.onclick = submitData;
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
                        ready: null,
                        refreshCaptcha:null
                    },
                    autoValidate: true,
                    autoRefresh: true,
                    divImg: null,
                    canvas: null,
                    controlRoot: null,
                    imgRoot: null,
                    tips: null,
                    modal: null,
                    icon: {
                        refresh: "captcha-icon-refresh",
                        succeed: "captcha-icon-succeed",
                        failed: "captcha-icon-failed"
                    }
                },
                tools: {},
                container: null,
                succeed: false,
                verifying: false
            };
            return func;
        }
        let captchas = (function () {
            let func = {};
            return func;
        })();
        let captchaData = mapData(), captchaResult = mapResult(), method = new template();
        mapObject(options, method.options);
        method.container = container || document.querySelector("#flyingrat");
        method.options.controlRoot = method.container.querySelector(".captcha-control");
        method.options.canvas = method.container.querySelector("canvas");
        method.options.divImg = method.container.querySelector(".captcha-img");
        method.options.imgRoot = method.container.querySelector(".captcha-control-image");
        method.options.tips = method.container.querySelector(".captcha-control-tips");
        method.options.modal = method.container.querySelector(".captcha-modal");
        let refbtn = method.container.querySelector(".captcha-control-refresh");
        if (refbtn) {
            refbtn.onclick = function () {
                if (isFunction(method.options.method.btn_refresh)) method.options.method.btn_refresh();
                else autoDrawImage();
            };
            addTool("refresh", refbtn);
        }
        defineProperty();
        addCaptcha("slidercaptcha", { init: captchaSlider, type: 2, ignoreAutoValidate: false });
        addCaptcha("pointcaptcha", { init: captchaClick, type: 3, ignoreAutoValidate: true });

        function defineProperty() {
            Object.defineProperties(method, {
                "name": { get: function () { return !!method.data.name?method.data.name.toLowerCase():null; } },
                "type": { get: function () { return method.current.type || null } },
                "data": { get: function () { return captchaData } },
                "result": { get: function () { return captchaResult; } },
                "captchas": { get: function () { return captchas; } },
                "current": { get: function () { return !!method.name?captchas[method.name]:null; } },
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
                "destroyCurrent": { get: function () { return destoryCaptcha; } },
                "InvokeCaptcha": { get: function () { return InvokeCaptcha; } },
                "refresh": { get: function () { return autoDrawImage; } },
                "validate": { get: function () { return verify; } },
                "validated": { get: function () { return verified; } },
                "validatePoint": { get: function () { return validatePoint; } },
                "addPoint": { get: function () { return addPoint; } },
                "createJsonData": { get: function () { return createJson; } },
                "delayedFunction": { get: function () { return delayedFuc; } },
                "autoValidate": { get: function () { return isAutoType(); } },
                "autoRefresh": { get: function () { return method.options.autoRefresh; } },
                "updateTips": { get: function () { return updateTips; } },
                "show": { get: function () { return rootShow; } },
                "hidden": { get: function () { return rootHidden; } },
                "modal": { get: function () { return { show: showModal, hidden: hiddenModal } } },
                "icon": { get: function () { return { show: function () { showIcon(null, false); }, hidden: function () { showIcon(null, true); } } } },
                "showIcon": { get: function () { return showIcon; } }
            })
        }

        function initCaptcha() {
            clearPoints();
            InvokeCaptcha("before");
            captchaResult = mapResult();
            method.succeed = false;
            method.verifying = false;
            method.icon.hidden();
            hiddenModal();
        }

        function destoryCaptcha() {
            if (!method.current) return;
            InvokeCaptcha("exit");
        }

        function destroy() {
            InvokeCaptcha("exit", false);
            clearPoints();
            method = null;
        }

        function addTool(key, tool) {
            if (!key) return false;
            method.tools[key] = tool;
        }

        function autoDrawImage() {
            if (!method.options.url) return;
            destoryCaptcha();
            if (isFunction(method.options.refreshCaptcha)) return excuteFunc(method.options.refreshCaptcha);
            try {
                let ajax = Ajax();
                ajax.get(method.options.url, function (data) {
                    drawImage(data);
                })
            } catch (e) {
            }
        }
        function drawImage(data) {
            if (!data) { delayedFuc(0.1,autoDrawImage);return;}
            let canvas = method.options.canvas;
            canvas == null ? drawDivImage(data) : drawCanvasImage(data);
        }
        function drawDivImage(data) {
            if (isFunction(method.options.method.setData)) data = method.options.method.setData(data);
            data = mapData(data);
            captchaData = data;
            initCaptcha();
            if (!hasShowCaptcha()) {
                show(container, true);
                return readyCaptcha();
            }
            show(container,false);
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
            if (isFunction(method.options.method.setData)) data = method.options.method.setData(data);
            data = mapData(data);
            captchaData = data;
            initCaptcha();
            if (!hasShowCaptcha()) {
                show(container, true);
                return readyCaptcha();
            }
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
            let captcha = { bindCaptcha: options.init(method), type: type, ignoreAutoValidate: !!options.ignoreAutoValidate };
            method.captchas[key.toLowerCase()] = captcha;
            return captcha;
        }
        function updateOptions(key, options) {
            if (!options) return false;
            let captcha = method.captchas[key.toLowerCase()];
            if (!captcha) return false;
            mapObject(options, captcha.options);
            return captcha;
        }
        function InvokeCaptcha(name, single) {
            if (typeof name != "string") return;
            single = isNaN(single) ? true : !!single;
            if (single && !!method.current) return excuteFunc(method.current.bindCaptcha[name]);
            Object.keys(captchas).forEach(function (key) {
                excuteFunc(captchas[key].bindCaptcha[name]);
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
            if (isFunction(method.options.method.validatePoint)) {
                return !!method.options.method.validatePoint(x, y,method);
            } else {
                return method.points.length < method.maxPoint;
            }
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

        function verify() {
            if (!InvokeCaptcha("allowVerify")) return;
            method.verifying = true;
            excuteFunc("validate");
            if (isFunction(method.options.method.validate)) {
                let isValid = method.options.method.validate(verified, method);
                if (isValid) return;
            }
            if (method.autoValidate) requestVerfiy();
        }
        function requestVerfiy() {
            let redata = method.current.bindCaptcha.jsonData();
            if (!redata.succeed) return;
            let data = method.data;
            if (!method.verifying) method.verifying = true;
            if (isFunction(method.options.method.setRequestData)) method.options.method.setRequestData(redata.data, data);
            if (!data.validate) return;
            try {
                let ajax = Ajax();
                ajax.post(data.validate, redata.data, function (data) {
                    verified(data);
                })
            } catch (e) {
                method.verifying = false;
            }
        }
        function verified(data) {
            if (!method) return;
            let udata = null;
            if (isFunction(method.options.method.setResult)) udata = method.options.method.setResult(data);
            captchaResult = mapResult(udata || data);
            method.succeed = captchaResult.succeed;
            if (captchaResult.refresh && (method.options.autoRefresh || isFunction(method.options.allowRefresh) && method.options.allowRefresh(captchaResult, method))) {
                delayedFuc(1, autoDrawImage);
                return;
            }
            excuteFunc(method.current.bindCaptcha.validated);
            if (isFunction(method.options.method.validated)) {
                method.options.method.validated(captchaResult, method.current.bindCaptcha.finished,method);
            } else {
                excuteFunc(method.current.bindCaptcha.finished);
            }
            clearPoints();
            method.verifying = false;
        }

        function mapResult(data, options) {
            let innerData = { token: null, succeed: false, refresh: false };
            if (!data) return innerData;
            let callback = keyMap = null;
            if (options) {
                callback = options.callback || null;
                keyMap = options.keyMap || null;
            }
            return mapObject(data, innerData, callback, keyMap);
        }
        function mapData(data,options) {
            let innerData = { name:null,index: null, change: null, width: 0, height: 0, col: 0, row: 0, x: 0, y: 0, tk: null, bgGap: null, gap: null, full: null, validate: null, isAction: false, tips: null, tw: 0, th: 0, type: 0};
            if (!data) return innerData;
            let callback = keyMap = null;
            if (options) {
                 callback = options.callback || null;
                 keyMap = options.keyMap || null;
            }
            return mapObject(data, innerData, callback, keyMap);
        }

        function hasShowCaptcha() {
            return method.current.bindCaptcha.hasShow || false;
        }
        function showModal() {
            if (!hasShowCaptcha()) return;
            show(method.options.modal, false);
        }
        function rootShow() {
            show(method.container, false);
        }
        function rootHidden() {
            show(method.container, true);
        }
        function hiddenModal() {
            if (!hasShowCaptcha()) return;
            show(method.options.modal, true);
        }
        function isAutoType() {
            if (!method.current.ignoreAutoValidate) return true;
            return !!method.options.autoValidate;
        }

        function showIcon(icon,hidden) {
            if (icon && typeof icon != "string") return;
            hidden = isNaN(hidden) ? true : !!hidden;
            if (icon) return show(method.container.querySelector("." + icon), hidden);
            Object.values(method.options.icon).forEach(function (icon) {
                show(method.container.querySelector("." + icon), hidden);
            })
        }
        return method;
    }
    function createJson(succeed, data) {
        succeed = !!succeed;
        return {succeed:succeed,data:data}
    }
    function delayedFuc(second, func, callback) {
        second = isNaN(second) ? 0 : Number(second);
        setTimeout(function () {
            if (isFunction(func)) func();
            if (isFunction(callback)) callback();
        }, second * 1000)
    }
    function show(element, hidden) {
        if (!element) return;
        element.style.display = !!hidden ? "none" : "block";
    }

    function getNumberPx(number) {
        return parseInt(number) + "px";
    }

    function excuteFunc(func,arg) {
        if (!isFunction(func)) return;
        return func(arg);
    }
    function isFunction(func) {
        if (func && typeof func === "function") return true;
        return false;
    }

    function Ajax() {
        let func = {
            get: get,
            post:post
        }
        function get(url,callback,async) {
            let request = createXMLHttpRequest();
            if (!request) return;
            async = isNaN(async) ? true : async;
            request.open("GET", url, !!async);
            request.setRequestHeader("Content-Type", "application/json");
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200 || request.status == 304) {
                    // 从服务器获得数据 
                    callback.call(this, JSON.parse(request.responseText));
                }
            }
            request.send();
            return request;
        }
        function post(url,data,callback,async) {
            let request = createXMLHttpRequest();
            if (!request) return;
            async = isNaN(async) ? true : async;
            request.open("POST", url, !!async);
            request.setRequestHeader("Content-Type", "application/json");
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200 || request.status == 304) {
                    // 从服务器获得数据 
                    callback.call(this, JSON.parse(request.responseText));
                }
            }

            request.send(JSON.stringify(data));
            return request;
        }
        function createXMLHttpRequest() {
            var xmlHttp;
            // 适用于大多数浏览器，以及IE7和IE更高版本
            try {
                xmlHttp = new XMLHttpRequest();
            } catch (e) {
                // 适用于IE6
                try {
                    xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
                } catch (e) {
                    // 适用于IE5.5，以及IE更早版本
                    try {
                        xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (e) { }
                }
            }
            return xmlHttp;
        }

        return func;
    }

    /**
     * 
    * 实体数据复制
    * @param {any} source 原数据
    * @param {any} target 转换成的新实体
    * @param {Function} callback 对原实体中的每个属性进行回调处理
    * @param {Object} setting 指定源key数据 与 目标key的映射
     */
    function mapObject(source, target, callback, setting) {
        var hasSetting = setting && typeof setting == "object";
        var targetKeys = Object.keys(source);
        if (hasSetting) {
            Object.keys(setting).forEach(key => {
                targetKeys.forEach((source, index) => {
                    if (source == setting[key]) targetKeys.splice(index, 1);
                })
            });
        }
        //把源数据的值 映射到目标对象中
        targetKeys.forEach(key => {
            var value = source[key];
            if (callback && typeof callback == 'function') {
                value = callback(key, value, source);
            }
            target[key] = value;
        });

        //如果有映射配置 执行
        if (hasSetting) {
            Object.keys(source).forEach(key => {
                var targetSetting = setting[key];
                var type = typeof targetSetting;
                if (type == "object") {
                    if (targetSetting.key && targetSetting.callback && typeof targetSetting.callback == "function") {
                        target[targetSetting.key] = targetSetting.callback(source[key], source);
                    }
                }
                else if (type == "string") {
                    target[targetSetting] = source[key];
                }
            });
        }
        return target;
    }
    window.captcha = captcha;
})(window, jQuery)
