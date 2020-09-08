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
      - id: unk_or_unused1
        type: u4
      - id: unk_or_unused2
        type: u4
      - id: unk_or_unused3
        type: u4
enums:
  field_compression:
    0: field_compression_none
    1: field_compression_bitpacked
    2: field_compression_common_data
    3: field_compression_bitpacked_indexed
    4: field_compression_bitpacked_indexed_array
    5: field_compression_bitpacked_signed
