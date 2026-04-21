const I18N = {
    vi: {
        /* ── Nav ── */
        'nav.create'  : 'Nhập dữ liệu',
        'nav.history' : 'Lịch sử nhập',
        'nav.report'  : 'Báo cáo sản lượng',
        'nav.reportsvn' : 'Báo cáo SVN',
        
        /* ── Create page ── */
        'create.title'        : 'Nhập Dữ Liệu Sản lượng Sản Xuất',
        'create.subtitle'     : 'Điền đầy đủ thông tin bên dưới để ghi nhận dữ liệu vào hệ thống.',
        'create.card.title'   : 'Form Nhập Liệu',
        
        /* ── Labels ── */
        'label.vendor'      : 'Nhà cung cấp',
        'label.process'     : 'Công đoạn',
        'label.code'        : 'Mã',
        'label.type'        : 'Loại',
        'label.quantity'    : 'Số lượng',
        'label.datetime'    : 'Ngày & Giờ',
        'label.description' : 'Mô tả "tùy chọn"',
        'label.image'       : 'Hình ảnh (Chụp / Tải lên) "tùy chọn"',
        
        /* ── Select placeholders ── */
        'ph.vendor'   : '-- Chọn Nhà cung cấp --',
        'ph.process'  : '-- Chọn Công đoạn --',
        'ph.code'     : '-- Chọn Công đoạn trước --',
        'ph.type'     : '-- Chọn Loại --',
        
        /* ── Input placeholders ── */
        'ph.quantity'    : 'Nhập số lượng...',
        'ph.description' : 'Nhập ghi chú hoặc mô tả (tùy chọn)...',
        
        /* ── Hints ── */
        'hint.quantity'     : 'Nhập số nguyên dương',
        'hint.datetime'     : 'Mặc định: ngày/giờ hiện tại',
        'hint.code.default' : '⬆ Chọn Công đoạn để tải mã tương ứng',
        'hint.code.loading' : 'Đang tải mã...',
        'hint.code.empty'   : '⚠ Không tìm thấy mã cho công đoạn "{v}"',
        'hint.code.ok'      : '✔ {n} mã tìm thấy cho "{v}"',
        'hint.code.error'   : '❌ Tải mã thất bại. Vui lòng thử lại.',
        
        /* ── Upload ── */
        'upload.text' : 'Nhấn để tải lên hoặc chụp ảnh',
        'upload.hint' : 'Hỗ trợ JPG, PNG, WEBP · Tối đa 10 MB',
        
        /* ── Buttons ── */
        'btn.reset'   : '🔄 Đặt lại',
        'btn.save'    : '💾 Lưu Dữ liệu',
        'btn.cancel'  : '✖ Hủy',
        'btn.confirm' : '✔ Xác nhận',
        'btn.saving'  : '⏳ Đang lưu...',
        
        /* ── Modal ── */
        'modal.title'    : 'Xác nhận Lưu',
        'modal.subtitle' : 'Vui lòng kiểm tra thông tin trước khi lưu vào hệ thống.',
        
        /* ── Modal summary labels ── */
        'modal.vendor'   : 'Mã NCC',
        'modal.process'  : 'Công đoạn',
        'modal.code'     : 'Mã',
        'modal.type'     : 'Loại',
        'modal.quantity' : 'Số lượng',
        'modal.datetime' : 'Ngày & Giờ',
        'modal.desc'     : 'Mô tả',
        'modal.image'    : 'Hình ảnh',
        'modal.none'     : '(không có)',
        
        /* ── Validation messages ── */
        'val.vendor'   : 'Vui lòng chọn Nhà cung cấp.',
        'val.process'  : 'Vui lòng chọn Công đoạn.',
        'val.code'     : 'Vui lòng chọn Mã.',
        'val.type'     : 'Vui lòng chọn Loại.',
        'val.quantity' : 'Vui lòng nhập Số lượng hợp lệ (> 0).',
        'val.datetime' : 'Vui lòng chọn Ngày & Giờ.',
        
        /* ── Toast ── */
        'toast.network_error' : 'Lỗi kết nối đến máy chủ.',
        'toast.code_loading_error' : 'Tải mã thất bại. Vui lòng thử lại.',
        
        /* ── Night shift modal ── */
        'night.modal.title'      : 'Xác nhận & Chọn ca',
        'night.modal.subtitle'   : 'Kiểm tra thông tin và chọn ca trước khi lưu.',
        'night.banner'           : '🌙 Bạn đang nhập lúc {hh}:{mn} — Chọn ca để lưu đúng ngày',
        'night.tab.night'        : '🌙 Ca đêm hôm qua',
        'night.tab.night.sub'    : '23:00 — {date}',
        'night.tab.now'          : '☀️ Giờ hiện tại',
        'night.tab.now.sub'      : '{hh}:{mn} — hôm nay',
        
        /* ── Type options ── */
        'type.production' : 'Số lượng sản xuất',
        'type.manpower'   : 'Nhân lực',
        
        /* ── History page ── */
        'hist.title'       : 'Lịch sử Sản xuất',
        'hist.subtitle'    : 'Tất cả bản ghi đã nhập. Dùng bộ lọc để tìm kiếm.',
        'hist.filter.title': '🔍 Bộ lọc tìm kiếm',
        'hist.label.vendor'   : 'Nhà cung cấp',
        'hist.label.process'  : 'Công đoạn',
        'hist.label.type'     : 'Loại',
        'hist.label.from'     : 'Từ ngày',
        'hist.label.to'       : 'Đến ngày',
        'hist.all.vendors'    : 'Tất cả NCC',
        'hist.all.process'    : 'Tất cả công đoạn',
        'hist.all.types'      : 'Tất cả loại',
        'hist.btn.search'     : '🔍 Tìm kiếm',
        'hist.btn.clear'      : '✖ Xóa lọc',
        'hist.table.title'    : '📊 Danh sách bản ghi',
        'hist.btn.export'     : '⬇ Xuất Excel',
        'hist.col.id'         : '#ID',
        'hist.col.process'    : 'Công đoạn',
        'hist.col.product'    : 'Sản phẩm',
        'hist.col.vendor'     : 'NCC',
        'hist.col.type'       : 'Loại',
        'hist.col.qty'        : 'Số lượng',
        'hist.col.date'       : 'Ngày hoàn thành',
        'hist.col.desc'       : 'Mô tả',
        'hist.col.image'      : 'Hình ảnh',
        'hist.loading'        : 'Đang tải dữ liệu...',
        
        /* ── Report page ── */
        'report.title'    : 'Báo cáo Sản xuất',
        'report.filter.title' : '🔍 Bộ lọc & Xuất',
        'report.label.process'  : 'Công đoạn',
        'report.label.product'  : 'Sản phẩm',
        'report.label.vendor'   : 'Nhà cung cấp',
        'report.label.from'     : 'Từ ngày',
        'report.label.to'       : 'Đến ngày',
        'report.btn.apply'      : '🔍 Áp dụng',
        'report.btn.reset'      : '↺ Đặt lại',
        'report.btn.excel'      : '⬇ Excel',
        'report.all.process'    : 'Tất cả công đoạn',
        'report.all.product'    : 'Tất cả sản phẩm',
        'report.all.vendor'     : 'Tất cả NCC',
        'report.all.operations' : 'Tất cả sản phẩm',
        'report.stat.qty'       : 'Sản lượng SX',
        'report.stat.qty.sub'   : 'Chỉ tính SX Qty',
        'report.stat.records'   : 'Bản ghi',
        'report.stat.records.sub' : 'Tất cả bản ghi',
        'report.stat.vendors'   : 'Nhà cung cấp',
        'report.stat.vendors.sub' : 'NCC đang hoạt động',
        'report.stat.processes' : 'Công đoạn',
        'report.stat.processes.sub' : 'Công đoạn hoạt động',
        'report.chart.by_process'       : 'Sản lượng theo Công đoạn',
        'report.chart.by_process.sub'   : 'SX Qty theo Công đoạn',
        'report.chart.by_vendor'        : 'Sản lượng theo NCC',
        'report.chart.by_vendor.sub'    : 'Phân bổ theo Nhà cung cấp',
        'report.chart.by_product'       : 'Sản lượng theo Sản phẩm',
        'report.chart.by_product.sub'   : 'Top sản phẩm theo SX Qty',
        'report.chart.manpower'         : 'Nhân lực trung bình — theo Công đoạn',
        'report.chart.daily_process'    : 'Xu hướng sản lượng hàng ngày — theo Công đoạn',
        'report.chart.daily_process.sub': 'SX Qty mỗi ngày theo công đoạn',
        'report.chart.daily_vendor'     : 'Xu hướng sản lượng hàng ngày — theo NCC',
        'report.chart.daily_vendor.sub' : 'SX Qty mỗi ngày theo nhà cung cấp',
        'report.detail.title'           : 'Chi tiết theo Nhà cung cấp',
        'report.detail.sub'             : 'Mở rộng để xem bản ghi sản xuất',
        'report.detail.records'         : '{n} bản ghi',
        'report.empty.hint'             : 'Áp dụng bộ lọc để tải dữ liệu báo cáo.',
        'report.nodata'                 : 'Không có dữ liệu',
        'report.nodata.manpower'        : 'Không có dữ liệu nhân lực',
        'report.nodata.display'         : 'Không có dữ liệu hiển thị',
        'report.loading'                : 'Đang tải báo cáo...',
        'report.col.no'       : 'STT',
        'report.col.date'     : 'Ngày',
        'report.col.process'  : 'Công đoạn',
        'report.col.product'  : 'Sản phẩm',
        'report.col.type'     : 'Loại',
        'report.col.qty'      : 'Số lượng',
        'report.col.desc'     : 'Mô tả',
        'report.export.nodata' : 'Không có dữ liệu để xuất.',
        'report.export.success': 'Đã xuất {n} bản ghi ({p} SX Qty).',
        'report.export.fail'   : 'Xuất file thất bại.',
        'report.error.load'    : 'Tải dữ liệu báo cáo thất bại.',
    },
    
    en: {
        /* ── Nav ── */
        'nav.create'  : 'Create',
        'nav.history' : 'History',
        'nav.report'  : 'Report',
        'nav.reportsvn' : 'SVN Report',
        
        /* ── Create page ── */
        'create.title'        : 'Production Data Entry',
        'create.subtitle'     : 'Fill in all the information below to record data into the system.',
        'create.card.title'   : 'Entry Form',
        
        /* ── Labels ── */
        'label.vendor'      : 'Vendor',
        'label.process'     : 'Process',
        'label.code'        : 'Code',
        'label.type'        : 'Type',
        'label.quantity'    : 'Quantity',
        'label.datetime'    : 'Date & Time',
        'label.description' : 'Description "optional"',
        'label.image'       : 'Image (Photo / Upload) "optional"',
        
        /* ── Select placeholders ── */
        'ph.vendor'   : '-- Select Vendor --',
        'ph.process'  : '-- Select Process --',
        'ph.code'     : '-- Select Process first --',
        'ph.type'     : '-- Select Type --',
        
        /* ── Input placeholders ── */
        'ph.quantity'    : 'Enter quantity...',
        'ph.description' : 'Enter additional notes or description (optional)...',
        
        /* ── Hints ── */
        'hint.quantity'     : 'Enter a positive integer',
        'hint.datetime'     : 'Default: current date/time',
        'hint.code.default' : '⬆ Select a Process to load matching codes',
        'hint.code.loading' : 'Loading matching codes...',
        'hint.code.empty'   : '⚠ No codes found for process "{v}"',
        'hint.code.ok'      : '✔ {n} code(s) matched for "{v}"',
        'hint.code.error'   : '❌ Failed to load codes. Please try again.',
        
        /* ── Upload ── */
        'upload.text' : 'Tap to upload or take a photo',
        'upload.hint' : 'Supports JPG, PNG, WEBP · Max 10 MB',
        
        /* ── Buttons ── */
        'btn.reset'   : '🔄 Reset',
        'btn.save'    : '💾 Save Data',
        'btn.cancel'  : '✖ Cancel',
        'btn.confirm' : '✔ Confirm',
        'btn.saving'  : '⏳ Saving...',
        
        /* ── Modal ── */
        'modal.title'    : 'Confirm Save',
        'modal.subtitle' : 'Please review the information before saving to the system.',
        
        /* ── Modal summary labels ── */
        'modal.vendor'   : 'Vendor Code',
        'modal.process'  : 'Process',
        'modal.code'     : 'Code',
        'modal.type'     : 'Type',
        'modal.quantity' : 'Quantity',
        'modal.datetime' : 'Date & Time',
        'modal.desc'     : 'Description',
        'modal.image'    : 'Image',
        'modal.none'     : '(none)',
        
        /* ── Validation messages ── */
        'val.vendor'   : 'Please select a Vendor.',
        'val.process'  : 'Please select a Process.',
        'val.code'     : 'Please select a Code.',
        'val.type'     : 'Please select a Type.',
        'val.quantity' : 'Please enter a valid Quantity (> 0).',
        'val.datetime' : 'Please select a Date & Time.',
        
        /* ── Toast ── */
        'toast.network_error'      : 'An error occurred while connecting to the server.',
        'toast.code_loading_error' : 'Failed to load codes. Please try again.',
        
        /* ── Night shift modal ── */
        'night.modal.title'      : 'Confirm & Select Shift',
        'night.modal.subtitle'   : 'Review information and select the correct shift before saving.',
        'night.banner'           : '🌙 You are entering at {hh}:{mn} — Select shift to save to the correct date',
        'night.tab.night'        : '🌙 Last night shift',
        'night.tab.night.sub'    : '23:00 — {date}',
        'night.tab.now'          : '☀️ Current time',
        'night.tab.now.sub'      : '{hh}:{mn} — today',
        
        /* ── Type options ── */
        'type.production' : 'Production Quantity',
        'type.manpower'   : 'Man Power',
        
        /* ── History page ── */
        'hist.title'       : 'Production History',
        'hist.subtitle'    : 'All records entered into the system. Use filters to search.',
        'hist.filter.title': '🔍 Search Filters',
        'hist.label.vendor'   : 'Vendor',
        'hist.label.process'  : 'Process',
        'hist.label.type'     : 'Type',
        'hist.label.from'     : 'From Date',
        'hist.label.to'       : 'To Date',
        'hist.all.vendors'    : 'All Vendors',
        'hist.all.process'    : 'All Process',
        'hist.all.types'      : 'All Types',
        'hist.btn.search'     : '🔍 Search',
        'hist.btn.clear'      : '✖ Clear',
        'hist.table.title'    : '📊 Record List',
        'hist.btn.export'     : '⬇ Export Excel',
        'hist.col.id'         : '#ID',
        'hist.col.process'    : 'Process',
        'hist.col.product'    : 'Product',
        'hist.col.vendor'     : 'Vendor',
        'hist.col.type'       : 'Type',
        'hist.col.qty'        : 'Quantity',
        'hist.col.date'       : 'Date Finished',
        'hist.col.desc'       : 'Description',
        'hist.col.image'      : 'Image',
        'hist.loading'        : 'Loading data...',
        
        /* ── Report page ── */
        'report.title'    : 'Production Report',
        'report.filter.title' : '🔍 Filter & Export',
        'report.label.process'  : 'Process',
        'report.label.product'  : 'Product',
        'report.label.vendor'   : 'Vendor',
        'report.label.from'     : 'From Date',
        'report.label.to'       : 'To Date',
        'report.btn.apply'      : '🔍 Apply',
        'report.btn.reset'      : '↺ Reset',
        'report.btn.excel'      : '⬇ Excel',
        'report.all.process'    : 'All Process',
        'report.all.product'    : 'All Products',
        'report.all.vendor'     : 'All Vendors',
        'report.all.operations' : 'All Operations',
        'report.stat.qty'       : 'Production Qty',
        'report.stat.qty.sub'   : 'Production Qty only',
        'report.stat.records'   : 'Records',
        'report.stat.records.sub' : 'All entries found',
        'report.stat.vendors'   : 'Vendors',
        'report.stat.vendors.sub' : 'Active vendors',
        'report.stat.processes' : 'Processes',
        'report.stat.processes.sub' : 'Active processes',
        'report.chart.by_process'       : 'Qty by Process',
        'report.chart.by_process.sub'   : 'Production Qty Process',
        'report.chart.by_vendor'        : 'Qty by Vendor',
        'report.chart.by_vendor.sub'    : 'Distribution Across Suppliers',
        'report.chart.by_product'       : 'Qty by Product',
        'report.chart.by_product.sub'   : 'Top Products By Production Qty',
        'report.chart.manpower'         : 'Average Man Power — by Process',
        'report.chart.daily_process'    : 'Daily Quantity Trend — by Process',
        'report.chart.daily_process.sub': 'Production Qty per day by process',
        'report.chart.daily_vendor'     : 'Daily Quantity Trend — by Vendor',
        'report.chart.daily_vendor.sub' : 'Production Qty per day for each supplier',
        'report.detail.title'           : 'Detail by Vendor',
        'report.detail.sub'             : 'Expand each vendor to see production records',
        'report.detail.records'         : '{n} records',
        'report.empty.hint'             : 'Apply a filter to load the report data.',
        'report.nodata'                 : 'No data',
        'report.nodata.manpower'        : 'No Man Power data',
        'report.nodata.display'         : 'No data to display',
        'report.loading'                : 'Loading report...',
        'report.col.no'       : '#',
        'report.col.date'     : 'Date',
        'report.col.process'  : 'Process',
        'report.col.product'  : 'Product',
        'report.col.type'     : 'Type',
        'report.col.qty'      : 'Qty',
        'report.col.desc'     : 'Description',
        'report.export.nodata' : 'No data to export.',
        'report.export.success': 'Exported {n} records ({p} Production Qty).',
        'report.export.fail'   : 'Export failed.',
        'report.error.load'    : 'Failed to load report data.',
    }
};

/* ── Helpers ─────────────────────────────────────────── */

let _currentLang = 'en';

/** Translate a key, with optional {v} / {n} interpolation */
function t(key, vars) {
    const dict = I18N[_currentLang] || I18N.en;
    let str = dict[key] ?? I18N.en[key] ?? key;
    if (vars) {
        Object.entries(vars).forEach(([k, v]) => {
            str = str.replace(new RegExp(`\\{${k}\\}`, 'g'), v);
        });
    }
    return str;
}

/* ── DOM apply ───────────────────────────────────────── */

function applyLang(lang) {
    if (!I18N[lang]) lang = 'en';
    _currentLang = lang;
    localStorage.setItem('lang', lang);
    
    /* text content */
    document.querySelectorAll('[data-i18n]').forEach(el => {
        const key = el.getAttribute('data-i18n');
        const val = t(key);
        /* preserve required asterisk span inside labels */
        const req = el.querySelector('.required');
        if (req) {
            el.childNodes[0].textContent = val + ' ';
        } else {
            el.textContent = val;
        }
    });
    
    /* placeholder */
    document.querySelectorAll('[data-i18n-ph]').forEach(el => {
        el.placeholder = t(el.getAttribute('data-i18n-ph'));
    });
    
    /* flag buttons */
    document.querySelectorAll('.lang-btn').forEach(btn => {
        btn.classList.toggle('active', btn.dataset.lang === lang);
    });
    
    /* notify pages (e.g. for Select2 re-init) */
    document.dispatchEvent(new CustomEvent('langChanged', { detail: { lang } }));
}

function setLang(lang) {
    applyLang(lang);
}

/* ── Auto-init on DOM ready ──────────────────────────── */
document.addEventListener('DOMContentLoaded', () => {
    const saved = localStorage.getItem('lang') || 'en';
    applyLang(saved);
});