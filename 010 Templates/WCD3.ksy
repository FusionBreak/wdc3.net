meta:
  id: wdc3
  application: World of Warcraft
  file-extension: db2
  endian: le
seq:
  - id: header
    type: header
  - id: sections_headers
    type: section_header
    repeat: expr
    repeat-expr: header.section_count
  - id: field_structures
    type: field_structure
    repeat: expr
    repeat-expr: header.total_field_count
  - id: field_storage_infos
    type: field_storage_info
    repeat: expr
    repeat-expr: header.field_storage_info_size / 24
  - id: pallet_data
    size: header.pallet_data_size
    if: header.pallet_data_size > 0
  - id: common_data
    size: header.common_data_size
    if: header.common_data_size > 0
  - id: sections
    type: section(_index)
    repeat: expr
    repeat-expr: header.section_count
types:
  header:
    seq:
      - id: magic
        type: str
        size: 4
        encoding: ASCII
      - id: record_count
        type: u4
      - id: field_count
        type: u4
      - id: record_size
        type: u4
      - id: string_table_size
        type: u4
      - id: table_hash
        type: u4
      - id: layout_hash
        type: u4
      - id: min_id
        type: u4
      - id: max_id
        type: u4
      - id: locale
        type: u4
      - id: flags
        type: u2
      - id: id_index
        type: u2
      - id: total_field_count
        type: u4
      - id: bitpacked_data_offset
        type: u4
      - id: lookup_column_count
        type: u4
      - id: field_storage_info_size
        type: u4
      - id: common_data_size
        type: u4
      - id: pallet_data_size
        type: u4
      - id: section_count
        type: u4
  field_structure:
    seq:
      - id: size
        type: s2
      - id: position
        type: u2
  section_header:
    seq:
      - id: tact_key_hash
        type: u8
      - id: file_offset
        type: u4
      - id: record_count
        type: u4
      - id: string_table_size
        type: u4
      - id: offset_records_end
        type: u4
      - id: id_list_size
        type: u4
      - id: relationship_data_size
        type: u4
      - id: offset_map_id_count
        type: u4
      - id: copy_table_count
        type: u4
  field_storage_info:
    seq:
      - id: field_offset_bits
        type: u2
      - id: field_size_bits
        type: u2
      - id: additional_data_size
        type: u4
      - id: storage_type
        type: u4
        enum: field_compression
      - id: data
        type:
          switch-on: storage_type
          cases:
            field_compression::field_compression_none: default_data
            field_compression::field_compression_bitpacked: bit_packed_data
            field_compression::field_compression_common_data: common_data
            field_compression::field_compression_bitpacked_indexed: bit_packed_indexed_data
            field_compression::field_compression_bitpacked_indexed_array: bit_packed_indexed_array_data
            field_compression::field_compression_bitpacked_signed: bit_packed_data
    types:
      default_data:
        seq:
          - id: unk_or_unused1
            type: u4
          - id: unk_or_unused2
            type: u4
          - id: unk_or_unused3
            type: u4
      bit_packed_data:
        seq:
          - id: bitpacking_size_bits
            type: u4
          - id: bitpacking_offset_bits
            type: u4
          - id: flags
            type: u4
      bit_packed_indexed_data:
        seq:
          - id: bitpacking_size_bits
            type: u4
          - id: bitpacking_offset_bits
            type: u4
          - id: unk_or_unused3
            type: u4
      bit_packed_indexed_array_data:
        seq:
          - id: bitpacking_size_bits
            type: u4
          - id: bitpacking_offset_bits
            type: u4
          - id: array_count
            type: u4
      common_data:
        seq:
          - id: default_value
            type: u4
          - id: unk_or_unused2
            type: u4
          - id: unk_or_unused3
            type: u4
  section:
    params:
      - id: section_index
        type: u4
    seq:
      - id: records
        type: record_data(_root.sections_headers[section_index].record_count)
        if: (_root.header.flags & 1) == 0
      - id: string_data
        size: _root.sections_headers[section_index].string_table_size
        if: (_root.header.flags & 1) == 0
      - id: variable_record_data
        size: _root.sections_headers[section_index].offset_records_end - _root.sections_headers[section_index].file_offset
        if: (_root.header.flags & 1) != 0
      - id: id_list
        type: u4
        repeat: expr
        repeat-expr: _root.sections_headers[section_index].id_list_size / 4
    types:
      record_data:
        params:
          - id: data_size
            type: u4
        seq:
          - id: data
            size: data_size
enums:
  field_compression:
    0: field_compression_none
    1: field_compression_bitpacked
    2: field_compression_common_data
    3: field_compression_bitpacked_indexed
    4: field_compression_bitpacked_indexed_array
    5: field_compression_bitpacked_signed
