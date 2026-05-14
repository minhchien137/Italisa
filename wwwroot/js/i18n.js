const I18N = {
    vi: {
        /* ── Nav ── */
        'nav.create'  : 'Nhập dữ liệu',
        'nav.history' : 'Lịch sử nhập',
        'nav.report'  : 'Báo cáo sản lượng',
        'nav.reportsvn' : 'Báo cáo tổng quan',
        'nav.defect.report' : 'Báo cáo tỷ lệ đạt',
        
        /* ── Create page ── */
        'create.title'        : 'Nhập Dữ Liệu Sản lượng Sản Xuất',
        'create.subtitle'     : 'Điền đầy đủ thông tin bên dưới để ghi nhận dữ liệu vào hệ thống.',
        'create.card.title'   : 'Form Nhập Liệu',
        
        /* ── Labels ── */
        'label.vendor'      : 'Nhà cung cấp',
        'label.process'     : 'Công đoạn',
        'label.code'        : 'Mã',
        'label.type'        : 'Loại',
        'label.color'       : 'Màu sắc',
        'label.quantity'    : 'Số lượng',
        'label.datetime'    : 'Ngày & Giờ',
        'label.description' : 'Mô tả "tùy chọn"',
        'label.image'       : 'Hình ảnh (Chụp / Tải lên) "tùy chọn"',
        
        /* ── Select placeholders ── */
        'ph.vendor'   : '-- Chọn Nhà cung cấp --',
        'ph.process'  : '-- Chọn Công đoạn --',
        'ph.code'     : '-- Chọn Công đoạn trước --',
        'ph.color'    : '-- Chọn Màu sắc --',
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
        'val.color'    : 'Vui lòng chọn Màu sắc.',  
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
        'type.defect'     : 'Lỗi sản phẩm',
        
        /* ── Defect ── */
        'label.defect'       : 'Loại lỗi',
        'ph.defect'          : '-- Chọn loại lỗi --',
        'val.defect'         : 'Vui lòng chọn Loại lỗi.',
        'modal.defect'       : 'Loại lỗi',
        'hint.defect.loading': 'Đang tải danh sách lỗi...',
        'hint.defect.error'  : '❌ Tải danh sách lỗi thất bại.',
        
        /* ── History page ── */
        'hist.title'       : 'Lịch sử Sản xuất',
        'hist.subtitle'    : 'Tất cả bản ghi đã nhập. Dùng bộ lọc để tìm kiếm.',
        'hist.filter.title': '🔍 Bộ lọc tìm kiếm',
        'hist.label.defect'   : 'Loại lỗi',
        'hist.all.defects'    : 'Tất cả lỗi',
        'hist.col.defect'     : 'Loại lỗi',
        'hist.label.vendor'   : 'Nhà cung cấp',
        'hist.label.process'  : 'Công đoạn',
        'hist.label.type'     : 'Loại',
        'hist.label.color'    : 'Màu sắc',
        'hist.label.product'  : 'Sản phẩm',
        'hist.label.from'     : 'Từ ngày',
        'hist.label.to'       : 'Đến ngày',
        'hist.all.vendors'    : 'Tất cả NCC',
        'hist.all.process'    : 'Tất cả công đoạn',
        'hist.all.colors'     : 'Tất cả màu sắc',
        'hist.all.products'   : 'Tất cả sản phẩm',
        'hist.all.types'      : 'Tất cả loại',
        'hist.btn.search'     : '🔍 Tìm kiếm',
        'hist.btn.clear'      : '✖ Xóa lọc',
        'hist.table.title'    : '📊 Danh sách bản ghi',
        'hist.btn.export'     : '⬇ Xuất Excel',
        'hist.col.id'         : '#ID',
        'hist.col.process'    : 'Công đoạn',
        'hist.col.color'      : 'Màu sắc',
        'hist.col.product'    : 'Sản phẩm',
        'hist.col.vendor'     : 'NCC',
        'hist.col.type'       : 'Loại',
        'hist.col.qty'        : 'Số lượng',
        'hist.col.date'       : 'Ngày hoàn thành',
        'hist.col.desc'       : 'Mô tả',
        'hist.col.image'      : 'Hình ảnh',
        'hist.loading'        : 'Đang tải dữ liệu...',
        'hist.empty'          : 'Không có bản ghi nào phù hợp với bộ lọc của bạn.',
        'hist.page.show'        : 'Hiển thị',
        'hist.page.showing'     : 'Đang hiển thị <strong>{from}–{to}</strong> / <strong>{total}</strong> bản ghi',
        'hist.page.total_pages' : '{pages} trang',
        'hist.page.no_records'  : 'Không có bản ghi',
        'hist.page.first'       : 'Trang đầu',
        'hist.page.prev'        : 'Trang trước',
        'hist.page.next'        : 'Trang tiếp',
        'hist.page.last'        : 'Trang cuối',
        
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
        'report.col.color'    : 'Màu sắc',
        'report.col.type'     : 'Loại',
        'report.col.qty'      : 'Số lượng',
        'report.col.desc'     : 'Mô tả',
        'report.export.nodata' : 'Không có dữ liệu để xuất.',
        'report.export.success': 'Đã xuất {n} bản ghi ({p} SX Qty).',
        'report.export.fail'   : 'Xuất file thất bại.',
        'report.error.load'    : 'Tải dữ liệu báo cáo thất bại.',
        
        /* ── Overview page ── */
        'overview.title'               : '📊 Tổng quan Sản xuất',
        'overview.subtitle'            : 'Báo cáo pivot theo sản phẩm — số lượng sản xuất theo trung tâm làm việc trong khoảng thời gian.',
        'overview.filter.title'        : '🔎 Bộ lọc & Xuất',
        'overview.label.from'          : 'Từ ngày',
        'overview.label.to'            : 'Đến ngày',
        'overview.btn.apply'           : '🔍 Áp dụng',
        'overview.btn.reset'           : '↺ Đặt lại',
        'overview.btn.excel'           : '⬇ Excel',
        'overview.stat.products'       : 'Tổng sản phẩm',
        'overview.stat.products.sub'   : 'Sản phẩm riêng biệt',
        'overview.stat.totalqty'       : 'Tổng sản lượng',
        'overview.stat.totalqty.sub'   : 'Tổng theo tất cả TT làm việc',
        'overview.stat.wc'             : 'Trung tâm làm việc',
        'overview.stat.wc.sub'         : 'Cột đang hoạt động',
        'overview.stat.minqty'         : 'Tổng SL có thể vận chuyển',
        'overview.stat.minqty.sub'     : 'Tổng số lượng hàng có thể vận chuyển cho SVN',
        'overview.table.title'         : '📋 Ma trận sản xuất theo TTLV',
        'overview.table.sub'           : 'Số lượng sản xuất theo sản phẩm và trung tâm làm việc',
        'overview.search.ph'           : 'Tìm tên sản phẩm hoặc mã PN…',
        'overview.state.init.title'    : 'Chọn khoảng ngày và nhấn Áp dụng',
        'overview.state.init.sub'      : 'Báo cáo sẽ hiển thị số lượng sản xuất theo trung tâm làm việc.',
        'overview.state.empty.title'   : 'Không tìm thấy dữ liệu',
        'overview.state.empty.sub'     : 'Thử điều chỉnh khoảng ngày hoặc từ khóa tìm kiếm.',
        'overview.state.error.title'   : 'Lỗi tải dữ liệu',
        'overview.loading'             : 'Đang tải dữ liệu báo cáo…',
        'overview.tfoot.total'         : 'Tổng',
        'overview.rows'                : '{n} hàng',
        'overview.rows.shown'          : '{n} sản phẩm hiển thị',
        'overview.toast.no_to'         : 'Vui lòng chọn ngày kết thúc.',
        'overview.toast.date_order'    : 'Ngày bắt đầu phải ≤ ngày kết thúc.',
        'overview.toast.load_fail'     : 'Tải báo cáo thất bại: {msg}',
        'overview.toast.no_export'     : 'Không có dữ liệu để xuất. Hãy áp dụng bộ lọc trước.',
        'overview.toast.export_ok'     : 'Đã xuất {n} hàng thành công.',
    },
    
    en: {
        /* ── Nav ── */
        'nav.create'  : 'Create',
        'nav.history' : 'History',
        'nav.report'  : 'Report',
        'nav.reportsvn' : 'Overview',
        'nav.defect.report' : 'Yield Report',
        
        /* ── Create page ── */
        'create.title'        : 'Production Data Entry',
        'create.subtitle'     : 'Fill in all the information below to record data into the system.',
        'create.card.title'   : 'Entry Form',
        
        /* ── Labels ── */
        'label.vendor'      : 'Vendor',
        'label.process'     : 'Process',
        'label.code'        : 'Code',
        'label.color'       : 'Color',
        'ph.color'          : '-- Select Color --',
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
        'val.color'    : 'Please select a Color.',
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
        'type.defect'     : 'Defect',
        
        /* ── Defect ── */
        'label.defect'       : 'Defect Code',
        'ph.defect'          : '-- Select Defect --',
        'val.defect'         : 'Please select a Defect.',
        'modal.defect'       : 'Defect',
        'hint.defect.loading': 'Loading defect list...',
        'hint.defect.error'  : '❌ Failed to load defects.',
        
        /* ── History page ── */
        'hist.title'       : 'Production History',
        'hist.subtitle'    : 'All records entered into the system. Use filters to search.',
        'hist.filter.title': '🔍 Search Filters',
        'hist.label.defect'   : 'Defect',
        'hist.all.defects'    : 'All Defects',
        'hist.col.defect'     : 'Defect',
        'hist.label.process'  : 'Process',
        'hist.label.vendor'   : 'Vendor',
        'hist.label.type'     : 'Type',
        'hist.label.color'    : 'Color',
        'hist.label.product'  : 'Product',
        'hist.label.from'     : 'From Date',
        'hist.label.to'       : 'To Date',
        'hist.all.vendors'    : 'All Vendors',
        'hist.all.process'    : 'All Process',
        'hist.all.colors'     : 'All Colors',
        'hist.all.products'   : 'All Products',
        'hist.all.types'      : 'All Types',
        'hist.btn.search'     : '🔍 Search',
        'hist.btn.clear'      : '✖ Clear',
        'hist.table.title'    : '📊 Record List',
        'hist.btn.export'     : '⬇ Export Excel',
        'hist.col.id'         : '#ID',
        'hist.empty'          : 'No records match your filters.',
        'hist.col.process'    : 'Process',
        'hist.col.product'    : 'Product',
        'hist.col.vendor'     : 'Vendor',
        'hist.col.color'      : 'Color',
        'hist.col.type'       : 'Type',
        'hist.col.qty'        : 'Quantity',
        'hist.col.date'       : 'Date Finished',
        'hist.col.desc'       : 'Description',
        'hist.col.image'      : 'Image',
        'hist.loading'        : 'Loading data...',
        'hist.page.show'        : 'Show',
        'hist.page.showing'     : 'Showing <strong>{from}–{to}</strong> of <strong>{total}</strong> records',
        'hist.page.total_pages' : '{pages} pages',
        'hist.page.no_records'  : 'No records',
        'hist.page.first'       : 'First page',
        'hist.page.prev'        : 'Previous page',
        'hist.page.next'        : 'Next page',
        'hist.page.last'        : 'Last page',
        
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
        'report.col.color'    : 'Color',
        'report.col.type'     : 'Type',
        'report.col.qty'      : 'Qty',
        'report.col.desc'     : 'Description',
        'report.export.nodata' : 'No data to export.',
        'report.export.success': 'Exported {n} records ({p} Production Qty).',
        'report.export.fail'   : 'Export failed.',
        'report.error.load'    : 'Failed to load report data.',
        
        /* ── Overview page ── */
        'overview.title'               : '📊 Production Overview',
        'overview.subtitle'            : 'Pivot report by item — qty produced per work-centre for a selected date range.',
        'overview.filter.title'        : '🔎 Filter & Export',
        'overview.label.from'          : 'From Date',
        'overview.label.to'            : 'To Date',
        'overview.btn.apply'           : '🔍 Apply',
        'overview.btn.reset'           : '↺ Reset',
        'overview.btn.excel'           : '⬇ Excel',
        'overview.stat.products'       : 'Total Products',
        'overview.stat.products.sub'   : 'Distinct items',
        'overview.stat.totalqty'       : 'Total Output Qty',
        'overview.stat.totalqty.sub'   : 'Sum across all WC',
        'overview.stat.wc'             : 'Work Centres',
        'overview.stat.wc.sub'         : 'Active columns',
        'overview.stat.minqty'         : 'Shippable quantity for SVN',
        'overview.stat.minqty.sub'     : 'Total shippable quantity for SVN',
        'overview.table.title'         : '📋 Work-Centre Production Matrix',
        'overview.table.sub'           : 'Qty produced per item per work centre',
        'overview.search.ph'           : 'Search product name or PN…',
        'overview.state.init.title'    : 'Select a date range and click Apply',
        'overview.state.init.sub'      : 'The report will load production quantities per work-centre.',
        'overview.state.empty.title'   : 'No data found',
        'overview.state.empty.sub'     : 'Try adjusting the date range or search term.',
        'overview.state.error.title'   : 'Error loading data',
        'overview.loading'             : 'Loading report data…',
        'overview.tfoot.total'         : 'Total',
        'overview.rows'                : '{n} rows',
        'overview.rows.shown'          : '{n} product(s) shown',
        'overview.toast.no_to'         : 'Please select To Date.',
        'overview.toast.date_order'    : 'From Date must be ≤ To Date.',
        'overview.toast.load_fail'     : 'Failed to load report: {msg}',
        'overview.toast.no_export'     : 'No data to export. Apply the filter first.',
        'overview.toast.export_ok'     : 'Exported {n} rows successfully.',
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