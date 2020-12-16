﻿(function (window, $) {
    window.captcha = function (container, options) {
        let method = {
            options: {
                url: null,
                points: [],
                pointIcon: [],
                method: {
                    setRequestData: null,
                    validate: null,
                    validateCallback: null,
                    btn_refresh: null,
                    setData: null,
                    setResult: null,
                    ready: null
                },
                x: 0,
                y: 0,
                autoValidate: true,
                autoRefresh:true,
                isMove: false,
                divImg: null,
                canvas: null,
                slider: null,
                sliderBackground: null,
                sliderImg: null,
                sliderWidth: 0,
                controlRoot: null,
                sliderRoot: null,
                imgRoot: null,
                tips: null,
                data: null,
                css: {
                    point: "captcha-point captcha-points-"
                }
            },
            container: null,
            isValidate: false,
            verifying: false
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
        
        let refbtn = method.container.querySelector(".captcha-control-refresh");
        if (refbtn) refbtn.onclick = function () {
            if (isFunction(method.options.method.btn_refresh)) method.options.method.btn_refresh();
            else autoDrawImage();
        };
        let captchas = (function () {
            let func = {
                "slidercaptcha": { bindControl: bindControlSlider,type:2},//sliderCaptcha
                "pointcaptcha": { bindControl: bindControlClick ,type:3}//pointCaptcha}
            };
            return func;
        })();
        defineProperty();
        function defineProperty() {
            Object.defineProperties(method, {
                "name": { get: function () { return method.options.data.name.toLowerCase() || null; } },
                "type": { get: function () { return method.current.type || null } },
                "captchas": { get: function () { return captchas; } },
                "current": { get: function () { return captchas[method.name];} },
                "points": { get: function () { return method.options.points; } },
                "maxPoint": { get: function () { return method.options.data.x || 0; } },
                "pointIcons": { get: function () { return method.options.pointIcon; } },
                "bindCaptcha": { get: function () { return setCaptcha; } },
                "mapData": { get: function () { return mapData; } },
                "drawCaptcha": { get: function () { return drawImage; } },
                "destroy": { get: function () { return destroy; } },
                "refresh": { get: function () { return autoDrawImage; } },
                "validate": { get: function () { return requestVerfiy; } },
                "validated": { get: function () { return verified; } },
                "validatePoint": { get: function () { return validatePoint; } },
                "addPoint": { get: function () { return addPoint; } },
                "verify": { get: function () { return verify;}}
            })
        }

        function initCaptcha() {
            clearPoints();
            clearPointEvent();
            method.options.x = method.options.y = method.options.sliderWidth = 0;
            method.options.isMove = false;
            method.options.data = null;
            method.isValidate = false;
            method.verifying = false;
            method.options.sliderRoot.style.display = "none";
            method.options.sliderImg.style.display = "none";
        }
        function destroy() {
            initCaptcha();
            method.options.url = null;
            method = null;
        }
        function autoDrawImage() {
            $.get(method.options.url, null, function (data) {
                drawImage(data);
            })
        }
        function drawImage(data) {
            initCaptcha();
            if (!data) {
                autoDrawImage();
                return;
            }
            if (isFunction(method.options.method.setData)) data = method.options.method.setData(data);
            data = mapData(data);
            let canvas = method.options.canvas, container = method.container;
            method.options.data = data;
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
                executeReady();
            }
        };
        function drawDivImage(data) {
            let div = method.options.divImg, container = method.container;
            method.options.data = data;
            if (div == null) return;
            container.style.display = "block";
            let url = data.isAction ? data.bgGap + "?tk=" + data.tk : data.bgGap;
            let image = document.createElement("div");
            div.style.width = getNumberPx(data.width);
            div.style.height = getNumberPx(data.height);
            //div.style.backgroundImage = "url(" + url + ")";
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
            bindControl();
            executeReady();
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
            if (method.options.slider) method.options.slider.style.left = getNumberPx(mx);
            if (method.options.sliderImg) method.options.sliderImg.style.left = getNumberPx(mx);
            if (method.options.sliderBackground) method.options.sliderBackground.style.width = getNumberPx(mx + method.options.slider.clientWidth);
        };
        function moveUp(event) {
            if (!method.options.isMove) return;
            sliderPoint(parseInt(method.options.sliderImg.style.left), parseInt(method.options.sliderImg.style.top));
            method.options.isMove = false;
            if (method.options.slider) method.options.slider.style.left = 0;
            if (method.options.sliderImg) method.options.sliderImg.style.left = 0;
            if (method.options.sliderBackground) method.options.sliderBackground.style.width = 0;
        };

        function setCaptcha(key, options) {
            if (!options || typeof options !="object" || !key || !options.init&&!isFunction(options.init) ) return false;
            let type = isNaN(options.type) ? 0 : parseInt(options.type);
            let captcha = { bindControl: options.init, type: type };
            method.captchas[key.toLowerCase()] = captcha;
            return captcha;
        }
        function bindControl() {
            let span = document.createElement("span");
            span.innerText = method.options.data.tips;
            updateTips(span, true);
            let control = method.captchas[method.name];
            if (control) control.bindControl();
        }
        function bindControlSlider() {
            let $slider = method.options.slider, gapImg = method.options.sliderImg;
            gapImg.src = method.options.data.isAction ? method.options.data.gap + "?tk=" + method.options.data.tk : method.options.data.gap;
            gapImg.style.top = getNumberPx(method.options.data.y);
            gapImg.style.display = "block";
            gapImg.onload = function () {
                method.options.sliderRoot.style.display = "block";
                $slider.style.width = getNumberPx(gapImg.clientWidth);
                method.options.sliderWidth = method.options.sliderRoot.clientWidth - $slider.clientWidth;
                $slider.addEventListener("mousedown", moveDown);
            }
        };
        function bindControlClick() {
            method.options.imgRoot.onclick = clickPoint;
            if (!method.options.data.tw) return;

            let div = document.createElement("div");
            let bg = method.options.isAction ? method.options.data.bgGap + "?tk=" + method.options.data.tk : method.options.data.bgGap;
            div.style.width = getNumberPx(method.options.data.tw);
            div.style.height = getNumberPx(method.options.data.th);
            div.style.marginLeft = getNumberPx(5);
            div.style.backgroundImage = "url(" + bg + ")";
            div.style.backgroundPositionY = getNumberPx(-method.options.data.height);
            updateTips(div);
        }

        function executeReady() {
            if (isFunction(method.options.method.ready)) {
                method.options.method.ready();
            }
        }

        function updateTips(el, isNew) {
            if (isNew) method.options.tips.innerHTML = "";
            method.options.tips.appendChild(el)
        }

        function sliderPoint(x, y) {
            let result = validatePoint(x, y);
            if (result.validate) addPoint(x, y);
            verify(result);
        }
        function clickPoint(event) {
            let x = event.layerX, y = event.layerY;
            let result = validatePoint(x, y);
            if (result.validate) {
                let index = method.options.points.length + 1;
                let max =method.maxPoint;
                drawPoint(x, y, index, function (div, index) {
                    if (max== index-1 && isAutoType()) return;
                    div.style.top = getNumberPx(y - div.clientHeight);
                    div.style.left = getNumberPx(x - (div.clientWidth >> 1));
                    div.onclick = function (event) {
                        if (method.verifying && isAutoType()) return;
                        console.debug(index, method.verifying, isAutoType())
                        clearPoints(index-1);
                        event.stopPropagation();
                    }
                })
                addPoint(x, y);
            }
            verify(result);
        }

        function clearPointEvent() {
            method.options.imgRoot.onclick = null;
            window.onmouseup = null;
            window.onmousemove = null;
            if (method.options.slider.removeEventListener) {
                method.options.slider.removeEventListener("mousedown", moveDown);
            }
        }

        function validatePoint(x, y) {
            let custom = { custom: false, validate: false };
            if (method.verifying && isAutoType()) return custom;
            if (isFunction(method.options.method.validate)) {
                let result = method.options.method.validate(x, y,method);
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
        function drawPoint(x, y, index, callback) {
            let div = createNumber(x, y, index);
            method.options.imgRoot.appendChild(div);
            method.options.pointIcon.push(div);
            if (isFunction(callback)) callback(div, index);
            return div;
        }
        function createNumber(x, y, number) {
            number = isNaN(number) ? 0 : parseInt(number);
            let div = document.createElement("div");
            //div.style.top = getNumberPx(y - 33);
            //div.style.left = getNumberPx(x - 13);
            div.className = method.options.css.point + number;
            return div;
        }
        function verify(custom) {
            if (custom.custom || method.isValidate || method.maxPoint != method.points.length) return;
            method.verifying = true;
            if (isAutoType()) requestVerfiy();
            if (method.current.type != 3) requestVerfiy();
        }
        function requestVerfiy() {
            let data = method.options.data, points = method.points;
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
            if (isFunction(method.options.method.setResult)) data = method.options.method.setResult(data);
            data = mapResult(data);
            method.isValidate = data.succeed;
            if (data.refresh && !data.succeed && method.options.autoRefresh) {
                autoDrawImage();
                return;
            }
            if (isFunction(method.options.method.validateCallback)) method.options.method.validateCallback(data);
            delayedClearPoints(1, function () { if (!method) return; method.verifying = false; });
        }

        function clearPoints(index) {
            index = isNaN(index) ? 0 : parseInt(index);
            if (!method) return;
            let points = method.options.points, icons = method.options.pointIcon;
            method.options.points = points.slice(0, Math.max(index, 0));
            if (method.options.pointIcon && !method.options.pointIcon.length) return;
            method.options.pointIcon = icons.slice(0, Math.max(index, 0));
            icons.slice(Math.min(index, icons.length)).every(function (item) {
                item.remove();
                return true;
            });
        }
        function delayedClearPoints(second, callback) {
            setTimeout(function () {
                clearPoints();
                if (isFunction(callback)) callback();
            }, parseInt(second) * 1000)
        }

        function isFunction(func) {
            if (func && typeof func === "function") return true;
            return false;
        }
        function getNumberPx(number) {
            return parseInt(number) + "px";
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
        function isAutoType(auto) {
            return method.current.type == 3 && method.options.autoValidate;
        }
        return method;
    };
})(window, jQuery)
